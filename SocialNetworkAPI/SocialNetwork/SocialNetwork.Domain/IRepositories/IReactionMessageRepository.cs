namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionMessageRepository : IBaseRepository<ReactionMessageEntity>
    {
        Task<List<string>> GetReactionIdByMessageIdAsync(string messageId);
    }
}
