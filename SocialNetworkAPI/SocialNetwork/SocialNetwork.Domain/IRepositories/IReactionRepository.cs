using SocialNetwork.DTOs.DTOs;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository : IBaseRepository<ReactionEntity>  
    {
        //Task AddAsync(ReactionEntity entity);
        //Task<ReactionEntity> GetByIdAsync(string reactionId);
        //Task UpdateAsync(ReactionEntity entity);
        ////Task DeleteAsync(Guid UserId ,Guid reactionId);
        //Task<EmotionTypeEntity> GetByIDAsync(string id);
        Task<EmotionTypeEntity> GetByID2Async(string id);

        Task<List<ReactionByUser>> GetReactionUserByReactionIdAsync(List<string> reactionId, string userId);

        Task<ReactionEntity> GetReactionIdByMessageIdAndUserId(ReactionMessageRequest reactionMessage);
    }
}
