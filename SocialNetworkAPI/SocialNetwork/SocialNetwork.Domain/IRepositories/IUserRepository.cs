using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity?> GetByUserNameAsync(string userName);

        Task<UserEntity?> GetLoginAsync(LoginRequest loginRequest);

        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<UserEntity> GetUserInfor(string userId);

        Task<IEnumerable<UserSearchViewModel>> SearchUserAsync(SearchQuery query,string userId);

        Task<UserEntity> UpdateUserInforAsync(UserEntity userEntity);

        Task<int> GetTotalFriendAsync(string userId);
    }
}
