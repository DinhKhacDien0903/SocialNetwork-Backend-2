using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentViewModel>> GetCommentByPostIdAsycn(Guid postId);
        Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsycn(Guid parentCommentId);
        Task<CommentViewModel> AddCommentAsycn(CommentViewModel comment);
        Task DeleteCommentAsycn(Guid commentId);
        Task<CommentViewModel> UpdateCommentAsycm(Guid commentId, CommentViewModel comment);
    }
}
