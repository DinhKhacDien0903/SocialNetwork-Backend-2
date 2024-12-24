using SocialNetwork.Domain;
using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialNetworkdDataContext _context;

        public PostRepository(SocialNetworkdDataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PostEntity entity)
        {
            entity.User = await _context.Users.FindAsync(entity.UserID);
            await _context.Posts.AddAsync(entity);
        }

        public async Task<IEnumerable<AdminBrowsePostViewModel>> GetAdminBrowseAsync()
        {
            var postWait = await _context.Posts.Include(x => x.User)
                .Include(x => x.Images).OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDelete)
                .Select(x => new AdminBrowsePostViewModel
                {
                    PostID = x.PostID,
                    UserID = x.UserID,
                    Content = x.Content,
                    LastName = x.User.LastName,
                    FirstName = x.User.FirstName,
                    AvatarUrl = x.User.AvatarUrl,
                    //CreatedAt=x.CreatedAt,
                    Images = x.Images.Where(x => !x.IsDeleted).Select(x => new ImagesOfPostViewModel
                    {
                        ImgUrl = x.ImgUrl,
                    }).ToList()
                }).ToListAsync();

            return postWait;
        }



        public async Task<IEnumerable<PostViewModel>> GetAllAsync(string userId)
        {
            var posts = await _context.Posts
            .Include(x => x.User)
            .Include(x => x.Images)
            .Include(x => x.Reactions)
                .ThenInclude(rp => rp.Reaction)
                .ThenInclude(r => r.EmotionType)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDelete)
                .Select(x => new PostViewModel
                {
                    PostID = x.PostID,
                    UserID = x.UserID,
                    Content = x.Content,
                    LastName = x.User.LastName,
                    FirstName = x.User.FirstName,
                    AvatarUrl = x.User.AvatarUrl,
                    UserReaction = x.Reactions.Where(x => !x.Reaction.IsDeleted && x.Reaction.UserID == userId)
                    .Select(x => new EmotionViewModel
                    {
                        EmotionName = x.Reaction.EmotionType.EmotionName,
                        EmotionTypeID = x.Reaction.EmotionTypeID,
                    }).FirstOrDefault(),

                    Reactions = x.Reactions
                    .Where(x => !x.Reaction.IsDeleted)
                    .Select(rp => new ReactionPostViewModel
                    {
                        ReactionID = rp.ReactionID,
                        EmotionTypeID = rp.Reaction.EmotionType.EmotionTypeID,
                        EmotionName = rp.Reaction.EmotionType.EmotionName,
                        UserID = rp.Reaction.UserID
                    }).ToList(),

                    Images = x.Images
                    .Where(x => !x.IsDeleted)
                    .Select(img => new ImagesOfPostViewModel
                    {
                        ImgUrl = img.ImgUrl
                    }).ToList(),
                })
                .ToListAsync();
            return posts;
        }


        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<PostEntity> GetPostWithImagesAsync(Guid postId)
        {
            return await _context.Posts.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PostID == postId.ToString());
        }

        public async Task<PostEntity> GetByIDAsync(string id)
        {
            return await _context.Posts
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.PostID == id);
        }

        public async Task Delete(string postId)
        {
            var post = await GetByIDAsync(postId);
            if (post != null)
            {
                post.IsDelete = true;
                _context.Entry(post).Property(p => p.IsDelete).IsModified = true;
                await _context.SaveChangesAsync();
            }

        }


    }
}
