namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionMessageRepository : IBaseRepository<ReactionMessageEntity>
    {
        Task<string> GetReactionIdByMessageIdAsync(string messageId);
    }
}
