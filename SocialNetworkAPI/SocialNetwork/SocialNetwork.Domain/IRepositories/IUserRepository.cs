using SocialNetwork.DTOs.Request;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity?> GetByUserNameAsync(string userName);

        Task<UserEntity?> GetLoginAsync(LoginRequest loginRequest);

        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<UserEntity> GetUserInfor(string userId);

        Task<IEnumerable<UserEntity>> SearchUserAsync(SearchQuery query,string userId);

        Task<UserEntity> UpdateUserInforAsync(UserEntity userEntity);

        Task<int> GetTotalFriendAsync(string userId);
    }
}
