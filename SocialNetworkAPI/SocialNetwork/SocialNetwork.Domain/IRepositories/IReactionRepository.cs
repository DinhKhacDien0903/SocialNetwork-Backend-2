using SocialNetwork.DTOs.DTOs;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository : IBaseRepository<ReactionEntity>
    {
        Task<List<ReactionByUser>> GetReactionUserByReactionIdAsync(List<string> reactionId,string userId);

        Task<ReactionEntity> GetReactionIdByMessageIdAndUserId(ReactionMessageRequest reactionMessage);
    }
}
