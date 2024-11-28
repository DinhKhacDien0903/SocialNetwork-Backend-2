using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(
            IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, SearchConversation searchParam)
        {
            var conversation = await _conversationRepository.GetAllConversationAsync(userId, searchParam.TextSearch, searchParam.PageIndex, searchParam.PageSize, searchParam.IsTotalCount);

            return conversation;
        }
    }
}
