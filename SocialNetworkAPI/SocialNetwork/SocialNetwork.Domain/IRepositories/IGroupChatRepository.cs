namespace SocialNetwork.Domain.IRepositories
{
    public interface IGroupChatRepository : IBaseRepository<GroupChatEntity>
    {
        Task<GroupChatEntity> CreateGroupChatAsync(string userId, GroupChatEntity groupChat, List<string> members);
        bool IsGroupNameExist(string groupName);

        Task UpdateGroupChatAvatar(string GroupchatId, string Avatar, DateTime updateDatetime);
    }
}
