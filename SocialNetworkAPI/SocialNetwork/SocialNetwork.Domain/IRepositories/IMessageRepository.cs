namespace SocialNetwork.Domain.IRepositories
{
    public interface IMessageRepository : IBaseRepository<MessagesEntity>
    {
        Task<IEnumerable<MessagesEntity>> GetAllMessageByFriendIdAsync(string userId, string receiverId);
    }
}
