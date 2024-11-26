using SocialNetwork.DTOs.DTOs;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository
    {
        Task AddAsync(ReactionEntity entity);
        Task<ReactionEntity> GetByIdAsync(string reactionId);
        Task UpdateAsync(ReactionEntity entity);
        //Task DeleteAsync(Guid UserId ,Guid reactionId);
        Task<EmotionTypeEntity> GetByIDAsync(string id);
    }
}
