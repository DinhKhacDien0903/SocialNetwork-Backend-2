using SocialNetwork.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface IPostService
    {
        Task<IEnumerable<PostViewModel>> GetAllPostsAsync();

        Task<PostViewModel> GetPostByIdAsync(Guid postId);

        Task<PostRequest> CreatePostAsync(PostRequest post);

        Task<PostViewModel> UpdatePostAsync(PostViewModel post);

        Task<bool> DeletePostAsync(Guid postId);

        Task<IEnumerable<PostViewModel>> GetPostsByUserIdAsync(string userId);
    }

}
