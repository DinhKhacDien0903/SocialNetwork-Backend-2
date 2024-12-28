namespace SocialNetwork.Domain.IRepositories
{
    public interface IGroupChatMessageImageRepository : IBaseRepository<GroupChatMessageImageEntity>
    {
        Task<List<string>> GetAllImageByMessageId(string MessageId);
    }
}
