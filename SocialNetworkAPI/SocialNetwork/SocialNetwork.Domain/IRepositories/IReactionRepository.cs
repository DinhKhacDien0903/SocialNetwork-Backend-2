namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository : IBaseRepository<ReactionEntity>
    {
        Task<string> GetEmotionTypeByReactionIdAsync(string reactionId,string userId);
    }
}
