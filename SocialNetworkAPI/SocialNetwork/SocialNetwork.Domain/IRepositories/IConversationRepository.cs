using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IConversationRepository
    {
        public Task<IEnumerable<LatestConversationsResponse>> GetAllConversationAsync(string userId, string searchText, int skip, int take);
    }
}
