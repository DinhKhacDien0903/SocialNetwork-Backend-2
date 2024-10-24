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

        public async Task DeleteAsycn(Guid commentId)
        {
            var comment=await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                comment.IsDelete = true;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();      
                //_context.Comments.Remove(comment);  
            }

        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(Guid postId)
        {
            var comments= await _context.Comments.Where(x=>x.PostID==postId.ToString() && x.ParentCommentID==null && !x.IsDelete).Include(x=>x.User).OrderByDescending(x=>x.CreatedAt).ToListAsync();
            return comments;
        }

        public async Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(Guid parentCommentId)
        {
            var comment=await _context.Comments.Where(x=>x.ParentCommentID==parentCommentId.ToString() && !x.IsDelete).Include(x=>x.User).OrderBy(x=>x.CreatedAt).ToListAsync(); 
            return comment;
        }

        
        public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
        {
            var commentById = await _context.Comments.FirstOrDefaultAsync(c => c.CommentID == commentId.ToString() && !c.IsDelete);
            return commentById;
        }

        public async Task<CommentEntity> AddCommentAsync(CommentEntity commentEntity)
        {
            _context.Comments.Add(commentEntity);   
            await _context.SaveChangesAsync();
            return commentEntity;
        }

        async Task<CommentEntity> ICommentRepositories.UpdateAsycn(CommentEntity comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
