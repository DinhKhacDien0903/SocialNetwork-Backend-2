using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionGroupChatMessageRepository : BaseRepository<ReactionGroupChatMessageEntity>, IReactionGroupChatMessageRepository
    {
        private readonly SocialNetworkdDataContext _context;
        public ReactionGroupChatMessageRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetReactionIdByMessageIdAsync(string messageId)
        {
            return await _context.ReactionGroupChatMessages.Where(x => x.GroupChatMessageID.Equals(messageId))
                .Select(x => x.ReactionID)
                .ToListAsync();
        }

        public async Task<List<ReactionByUser>> GetReactionUserByReactionIdAsync(List<string> reactionIds, string userId)
        {
            return await _context.Reactions.Where(x => reactionIds.Contains(x.ReactionID))
                .Select(x => new ReactionByUser
                {
                    UserId = x.UserID,
                    EmotionType = x.EmotionTypeID,
                    ReactionId = x.ReactionID,
                })
                .ToListAsync();
        }
    }
}
