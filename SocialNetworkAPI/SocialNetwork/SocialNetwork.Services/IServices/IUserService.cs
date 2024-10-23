using SocialNetwork.DTOs.Authorize;

namespace SocialNetwork.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

        Task<UserViewModel> GetUserByIdAsync(string id);

        Task<bool> DeleteUserAsync(string id);

        string HashPassWord(string password);

        bool VerifyPassword(string hashedPassword, string providePassword);

        Task<UserViewModel>GetUserInforAsync(string userId);

        Task<IEnumerable<FriendViewModel>> GetFriendOnlinesAsync(string userId);
    }
}
