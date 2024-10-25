using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class CommentRepositories : BaseRepository<CommentEntity>, ICommentRepositories
    {
        private readonly SocialNetworkdDataContext _context;
        public CommentRepositories(SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CommentEntity> AddCommentAsync(CommentEntity commentEntity)
        {
            var comment = await _context.Comments.AddAsync(commentEntity);
            await _context.SaveChangesAsync();
            return commentEntity;
        }

        public async Task DeleteAsync(Guid commentId)
        {
            var commet = await GetCommentByIdAsync(commentId);
            if (commet != null)
            {
                commet.IsDelete = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(Guid postId)
        {
            var commnet = await _context.Comments.Where(x => x.PostID == postId && !x.IsDelete).Include(x => x.User).ToListAsync();
            return commnet;
        }

        public async Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(Guid parentCommentId)
        {
            var comment = await _context.Comments.Where(x => x.ParentCommentID == parentCommentId && !x.IsDelete).Include(x => x.User).ToListAsync();
            return comment;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}



