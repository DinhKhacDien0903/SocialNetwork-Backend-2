using SocialNetwork.DTOs.Response;
using SocialNetwork.DTOs.ViewModels;
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
        Task<CommentResultViewModel> GetCommentsByPostIdAsync(string postId);
        //Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(string parentCommentId);
        Task<CommentEntity> GetCommentByIdAsync(string commentId);
        Task AddCommentAsync(CommentEntity comment);
        Task DeleteCommentAsync(string commentId);
        Task UpdateCommentAsync(CommentEntity comment);
        Task<int> GetCommentCountByPostIdAsync(string postId);
        Task<int> GetCountComment(string postId);
    }
}
