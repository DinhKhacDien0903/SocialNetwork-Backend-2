
namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionMessageRepository : BaseRepository<ReactionMessageEntity>, IReactionMessageRepository
    {
        private readonly SocialNetworkdDataContext _context;
        public ReactionMessageRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetReactionIdByMessageIdAsync(string messageId)
        {
            return await _context.ReactionMessages.Where(x => x.MessageID.Equals(messageId))
                .Select(x => x.ReactionID)
                .ToListAsync();
        }
    }
}
