using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //var user=await _context.Users.FindAsync(enti)
            var posts = await _context.Posts
                .Include(x => x.User)
                .Include(x => x.Reaction)
                .ThenInclude(r => r.EmotionType)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new PostViewModel
                {
                    PostID = x.PostID,
                    UserID = x.UserID,
                    Content = x.Content,
                    LastName = x.User.LastName,
                    FirstName = x.User.FirstName,
                    AvatarUser = x.User.AvatarUrl,
                    CurrentEmotionId = x.Reaction.Any() ? x.Reaction.First().EmotionType.EmotionTypeID : null,
                    CurrentEmotionName = x.Reaction.Any() ? x.Reaction.First().EmotionType.EmotionName : null,
                    Reactions = x.Reaction.Select(r => new ReactionPostViewModel
                    {
                        ReactionID = r.ReactionID,
                        EmotionTypeID = r.EmotionType.EmotionTypeID
                    }).ToList(),
                    // Uncomment nếu cần kiểm tra phần Images
                    // Images = x.Images.Select(img => new ImagesOfPostViewModel
                    // {
                    //     ImageUrl = img.ImageUrl 
                    // }).ToList(),
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

        public async Task<IEnumerable<PostEntity>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.Posts.Where(p => p.UserID == userId).ToListAsync();
        }

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
           var post=await GetByIDAsync(Entity.PostID);
            if (post != null)
            {
                post.IsDelete = true;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

        }
    }
}
