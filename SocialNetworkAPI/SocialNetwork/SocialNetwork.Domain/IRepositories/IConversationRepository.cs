using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IConversationRepository
    {
        public Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount);
        public Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount);
    }
}
