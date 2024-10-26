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
    }
}
