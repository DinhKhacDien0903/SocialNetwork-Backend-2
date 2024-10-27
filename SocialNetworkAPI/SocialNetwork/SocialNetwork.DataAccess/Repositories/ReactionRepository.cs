

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

        public async Task<string> GetEmotionTypeByReactionIdAsync(string reactionId, string userId)
        {
            return await _context.Reactions.Where(x => x.ReactionID.Equals(reactionId) && x.UserID.Equals(userId))
                .Select(x => x.EmotionTypeID)
                .FirstOrDefaultAsync();
        }
    }
}
