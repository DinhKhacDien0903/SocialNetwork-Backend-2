using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostViewModel>> GetAllAsync(string userid);
        Task<IEnumerable<AdminBrowsePostViewModel>> GetAdminBrowseAsync();

        Task<PostEntity> GetByIDAsync(string id);

        Task AddAsync(PostEntity entity);

        //Task Update(PostEntity entity);

        Task Delete(string postId);

        Task SaveChangeAsync();

        //Task<IEnumerable<PostViewModel>> GetPostsByUserIdAsync(string userId);
    }
}
