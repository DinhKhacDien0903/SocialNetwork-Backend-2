using SocialNetwork.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SocialNetwork.DataAccess.Repositories
{
    public class PostRepository : BaseRepository<PostEntity>, IPostRepository
    {
        private readonly SocialNetworkdDataContext _context;

        public PostRepository(SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PostEntity> GetByIDPostAsync(Guid id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<PostEntity>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.Posts.Where(p => p.UserID == userId).ToListAsync();
        }

        //public async Task<PostEntity> GetPostWithImagesAsync(Guid postId)
        //{
        //    return await _context.Posts.Include(p => p.Images)
        //        .FirstOrDefaultAsync(p => p.PostID == postId);
        //}
    }
}
