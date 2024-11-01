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
            await _context.Posts.AddAsync(entity);
        }

        public async Task<PostEntity> GetByIDAsync(Guid id)
        {
            return await _context.Posts.FindAsync(id);
        }

        //public async Task<IEnumerable<PostViewModel>> GetAllAsync()
        //{
        //    var post = await _context.Posts.Include(x => x.User).Select(x => new PostViewModel
        //    {
        //        UserID = x.UserID,
        //        Content = x.Content,
        //        UserLastName = x.User.LastName,
        //        UserFirstName = x.User.FirstName,
        //        AvatarUser = x.User.AvatarUrl,
        //        CurrentEmotionId = x.Reaction.FirstOrDefault()?.EmotionType?.EmotionTypeID,
        //        CurrentEmotionName = x.Reaction.FirstOrDefault()?.EmotionType?.EmotionName,
        //        Reactions = x.Reaction.Select(r => new ReactionPostViewModel
        //        {
        //            ReactionID = r.ReactionID,
        //            EmotionTypeID = r.EmotionType.EmotionTypeID,
        //            EmotionTypeName = r.EmotionType.EmotionName
        //        }).ToList(),
        //        //CreatedAt = x.CreatedAt,
        //    }).ToListAsync();
        //    //var post= await  _context.Posts.ToListAsync();
        //    return post;
        //}


        public async Task<IEnumerable<PostViewModel>> GetAllAsync()
        {
            var posts = await _context.Posts
                .Include(x => x.User)
                .Include(x => x.Reaction) 
                .ThenInclude(r => r.EmotionType) 
                .Select(x => new PostViewModel
                {
                    PostID = Guid.Parse(x.PostID), 
                    UserID = x.UserID,
                    Content = x.Content,
                    UserLastName = x.User.LastName,
                    UserFirstName = x.User.FirstName,
                    AvatarUser = x.User.AvatarUrl,

                    CurrentEmotionId = x.Reaction.Any() ? x.Reaction.First().EmotionType.EmotionTypeID : null,
                    CurrentEmotionName = x.Reaction.Any() ? x.Reaction.First().EmotionType.EmotionName : null,
                    Reactions = x.Reaction.Select(r => new ReactionPostViewModel
                    {
                        ReactionID = r.ReactionID,
                        EmotionTypeID = r.EmotionType.EmotionTypeID,
                        EmotionTypeName = r.EmotionType.EmotionName
                    }).ToList(),


                    //CreatedAt = x.CreatedAt,
                    //Images = x.Images.Select(img => new ImagesOfPostViewModel
                    //{
                    //    ImageUrl = img.ImageUrl 
                    //}).ToList(),
                })
                .ToListAsync();

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

        public Task<PostEntity> GetByIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(PostEntity Entity)
        {
            throw new NotImplementedException();
        }
    }
}
