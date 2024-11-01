namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository
    {
        Task AddAsync(ReactionEntity entity);
        Task<ReactionEntity> GetByIdAsync(string reactionId);
        Task UpdateAsync(ReactionEntity entity);
        //Task DeleteAsync(Guid UserId ,Guid reactionId);
    }
}
