using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionGroupChatMessageRepository : IBaseRepository<ReactionGroupChatMessageEntity>
    {
        Task<List<string>> GetReactionIdByMessageIdAsync(string messageId);

        Task<List<ReactionByUser>> GetReactionUserByReactionIdAsync(List<string> reactionIds, string userId);
    }
}
