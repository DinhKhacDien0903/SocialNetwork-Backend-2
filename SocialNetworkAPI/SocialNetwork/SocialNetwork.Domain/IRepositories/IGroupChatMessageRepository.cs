namespace SocialNetwork.Domain.IRepositories
{
    public interface IGroupChatMessageRepository : IBaseRepository<GroupChatMessageEntity>
    {
        Task<IEnumerable<GroupChatMessageEntity>> GetAllMessageByGroupIdAsync(string groupId);
    }
}
