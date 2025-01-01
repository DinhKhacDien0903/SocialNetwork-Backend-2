using System.Text.RegularExpressions;

namespace SocialNetwork.Services.IServices
{
    public interface IGroupChatService
    {
        Task<GroupChatViewModel> CreateGroupChatAsync(string userId, GroupChatViewModel request);

        bool ValidateGroupChat(GroupChatViewModel groupChat);

        Task LeaveGroupChatAsync(string userId, string groupId);
    }
}
