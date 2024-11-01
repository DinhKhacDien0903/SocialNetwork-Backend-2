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
        Task<IEnumerable<PostViewModel>> GetAllAsync();

        Task<PostEntity> GetByIDAsync(Guid id);

        Task AddAsync(PostEntity entity);

        void Update(PostEntity entity);

        void Delete(PostEntity Entity);

        Task SaveChangeAsync();

        Task<IEnumerable<PostEntity>> GetPostsByUserIdAsync(string userId);
        Task<PostEntity> GetPostWithImagesAsync(Guid postId);
    }
}
