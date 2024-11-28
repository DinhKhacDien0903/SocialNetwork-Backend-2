using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.IServices
{
    public interface IConversationService
    {
        Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, SearchConversation searchParam);
        Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, SearchConversation searchParam);
    }
}
