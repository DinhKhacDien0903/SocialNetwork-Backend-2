

using SocialNetwork.DTOs.DTOs;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionRepository : BaseRepository<ReactionEntity>, IReactionRepository
    {
        private readonly SocialNetworkdDataContext _context;
        public ReactionRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
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

        public async Task<ReactionEntity> GetReactionIdByMessageIdAndUserId(ReactionMessageRequest param)
        {
            var result = from reaction in _context.Reactions
                         join reactionMessage in _context.ReactionMessages
                         on reaction.ReactionID equals reactionMessage.ReactionID
                         where reaction.UserID.Equals(param.SenderId) && reactionMessage.MessageID.Equals(param.MessageId)
                         select reaction;

            return await result.FirstOrDefaultAsync();
        }

        //optional
        public async Task<EmotionTypeEntity> GetByID2Async(string id)
        {
            return await _context.EmotionTypes.FindAsync(id);
        }

        public async Task AddAsync(ReactionEntity entity)
        {
            await _context.Reactions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ReactionEntity> GetByIdAsync(string id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            return reaction;
        }

        public async Task UpdateAsync(ReactionEntity entity)
        {
            _context.Reactions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}