using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface ICommentRepositories
    {
        Task<IEnumerable<CommentEntity>> GetAllAsync();
        Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(string postId);
        Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(string parentCommentId);
        Task<CommentEntity> GetCommentByIdAsync(string commentId);
        Task AddCommentAsync(CommentEntity comment);
        Task DeleteCommentAsync(string commentId);
        Task UpdateCommentAsync(CommentEntity comment);
    }
}
