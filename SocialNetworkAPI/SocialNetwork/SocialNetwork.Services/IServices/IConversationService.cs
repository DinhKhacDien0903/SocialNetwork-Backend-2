using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.IServices
{
    public interface IConversationService
    {
        Task<IEnumerable<LatestConversationsResponse>> GetAllConversationAsync(string userId, SearchConversation searchParam);
    }
}
