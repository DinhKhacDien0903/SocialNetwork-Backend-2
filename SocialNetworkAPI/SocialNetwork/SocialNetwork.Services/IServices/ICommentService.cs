using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentViewModel>> GetAllCommentAsync();
        Task<CommentResultViewModel> GetCommentByPostIdAsync(string postId);
        //Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsync(string parentCommentId);
        Task<CommentViewModel> GetCommentByIdAsync(string commentId);
        Task<CommentViewModel> AddCommentAsync(CommentRequest comment, string userId);
        Task DeleteCommentAsync(string commentId);
        Task UpdateCommentAsync(CommentViewModel comment);
        Task<int> GetCommentCountByPostIdAsync(string postId);

    }
}
