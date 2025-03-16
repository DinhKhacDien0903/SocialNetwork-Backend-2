using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Domain.IRepositories
{
        public interface IConversationRepository
        {
                public Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount);

                public Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount);

                public Task<BaseSearchGroupChatsRespone> GetGroupChatsAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount);

                public Task<BaseSearchGroupMemberResponse> GetGroupMembersAsync(string userId, string searchText, string GroupID, int pageIndex, int pageSize, bool isTotalCount);
        }
}
