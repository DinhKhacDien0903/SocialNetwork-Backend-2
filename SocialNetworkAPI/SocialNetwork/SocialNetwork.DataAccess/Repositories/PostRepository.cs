using SocialNetwork.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<PostEntity>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
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
                .FirstOrDefaultAsync(p => p.PostID == postId);
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
