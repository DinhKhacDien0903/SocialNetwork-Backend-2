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





        public async Task<IEnumerable<PostViewModel>> GetAllAsync()
        {
            var posts = await _context.Posts
            .Include(x => x.User)
            .Include(x => x.Images)
            .Include(x => x.Reactions)
                .ThenInclude(rp => rp.Reaction)
                .ThenInclude(r => r.EmotionType)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new PostViewModel
                {
                    PostID = x.PostID,
                    UserID = x.UserID,
                    Content = x.Content,
                    LastName = x.User.LastName,
                    FirstName = x.User.FirstName,
                    AvatarUrl = x.User.AvatarUrl,

                    EmotionTypeID = x.Reactions.Any(x => !x.Reaction.IsDeleted) ? x.Reactions.First().Reaction.EmotionType.EmotionTypeID : null,
                    EmotionName = x.Reactions.Any(x => !x.Reaction.IsDeleted) ? x.Reactions.First().Reaction.EmotionType.EmotionName : null,

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


            foreach (var post in posts)
            {
                Console.WriteLine($"PostID: {post.PostID}, UserLastName: {post.LastName}, UserFirstName: {post.FirstName}");
            }

            return posts;
        }


        public void Update(PostEntity entity)
        {
            _context.Posts.Update(entity);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        //public async Task<IEnumerable<PostViewModel>> GetPostsByUserIdAsync(string userId)
        //{
        //    var postUser = await _context.Posts
        //         .Include(x => x.User)
        //         .Include(x => x.Images)
        //             .ThenInclude(x => x.ImgUrl)
        //         .Include(x => x.Reactions)
        //             .ThenInclude(x => x.Reaction)
        //             .ThenInclude(x => x.EmotionType)
        //             .Where(x => !x.IsDelete && x.UserID==userId )
        //             .OrderByDescending(x => x.CreatedAt)
        //             .Select(x => new PostViewModel
        //             {
        //                 PostID = x.PostID,
        //                 UserID = x.UserID,
        //                 Content = x.Content,
        //                 FirstName = x.User.FirstName,
        //                 LastName = x.User.LastName,
        //                 AvatarUrl = x.User.AvatarUrl,

        //                 Reactions = x.Reactions
        //                 .Where(r => !r.Reaction.IsDeleted)
        //                 .Select(r => new ReactionPostViewModel
        //                 {
        //                     ReactionID = r.ReactionID,
        //                     UserID = r.Reaction.UserID,
        //                     EmotionTypeID = r.Reaction.EmotionTypeID,
        //                     EmotionName = r.Reaction.EmotionType.EmotionName
        //                 }).ToList(),
        //                 Images = x.Images.Where(x => !x.IsDeleted)
        //                  .Select(i => new ImagesOfPostViewModel
        //                  {
        //                      ImgUrl = i.ImgUrl,
        //                  }).ToList()
        //             }).ToListAsync();

        //    return postUser;

        //}

        public async Task<PostEntity> GetPostWithImagesAsync(Guid postId)
        {
            return await _context.Posts.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PostID == postId.ToString());
        }

        public async Task<PostEntity> GetByIDAsync(string id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async void Delete(PostEntity Entity)
        {
            var post = await GetByIDAsync(Entity.PostID);
            if (post != null)
            {
                post.IsDelete = true;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

        }
    }
}
