using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.IServices
{
    public interface IConversationService
    {
        Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, BaseSearch searchParam);

        Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, BaseSearch searchParam);

        Task<BaseSearchGroupChatsRespone> GetGroupChatsAsync(string userId, BaseSearch searchParam);

        Task<BaseSearchGroupMemberResponse> GetGroupMembersAsync(string userId, BaseSearch searchParam);
    }
}
