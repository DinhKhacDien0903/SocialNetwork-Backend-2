using SocialNetwork.DTOs.Request;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository : IBaseRepository<ReactionEntity>
    {
        Task<List<string>> GetEmotionTypeByReactionIdAsync(List<string> reactionId,string userId);

        Task<ReactionEntity> GetReactionIdByMessageIdAndUserId(ReactionMessageRequest reactionMessage);
    }
}
