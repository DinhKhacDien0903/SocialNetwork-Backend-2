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

        public async Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, BaseSearch searchParam)
        {
            var conversation = await _conversationRepository.GetAllConversationAsync(userId, searchParam.TextSearch, searchParam.PageIndex, searchParam.PageSize, searchParam.IsTotalCount);

            return conversation;
        }

        public async Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, BaseSearch searchParam)
        {
            var friends = await _conversationRepository.GetFriendsAsync(userId, searchParam.TextSearch, searchParam.PageIndex, searchParam.PageSize, searchParam.IsTotalCount);

            return friends;
        }

        public async Task<BaseSearchGroupChatsRespone> GetGroupChatsAsync(string userId, BaseSearch searchParam)
        {
            var groups = await _conversationRepository.GetGroupChatsAsync(userId, searchParam.TextSearch, searchParam.PageIndex, searchParam.PageSize, searchParam.IsTotalCount);

            return groups;
        }

        public async Task<BaseSearchGroupMemberResponse> GetGroupMembersAsync(string userId, BaseSearch searchParam)
        {
            var groupMembers = await _conversationRepository.GetGroupMembersAsync(userId, searchParam.TextSearch, searchParam.GroupChatId, searchParam.PageIndex, searchParam.PageSize, searchParam.IsTotalCount);

            return groupMembers;
        }
    }
}
