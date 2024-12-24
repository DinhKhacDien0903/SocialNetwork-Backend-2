using SocialNetwork.Domain;
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
        Task<PageResult<PostViewModel>> GetAllPostsAsync(string userId, int pageSize, int pageIndex);

        Task<IEnumerable<AdminBrowsePostViewModel>> GetAdminBrowseAsync();

        Task<PostViewModel> GetPostByIdAsync(string postId);

        Task<PostResponse> CreatePostAsync(PostRequest post,string userID);

        //Task<PostViewModel> UpdatePostAsync(PostViewModel post);

        Task<bool> DeletePostAsync(string postId);

        Task<PageResult<PostViewModel>> GetPostsByUserIdAsync(string userId, int pageSize, int pageIndex);
    }

}
