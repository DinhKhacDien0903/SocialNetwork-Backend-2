using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.Response;
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
        //Task<IEnumerable<EmotionRequest>> GetAllEmotionAsync();

        Task<PostViewModel> GetPostByIdAsync(string postId);

        Task<PostResponse> CreatePostAsync(PostRequest post,string userID);

        Task<PostViewModel> UpdatePostAsync(PostViewModel post);

        Task<bool> DeletePostAsync(string postId);

        Task<IEnumerable<PostViewModel>> GetPostsByUserIdAsync(string userId);
    }

}
