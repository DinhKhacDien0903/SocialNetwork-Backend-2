
namespace SocialNetwork.DataAccess.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        private readonly SocialNetworkdDataContext _context;
        public RefreshTokenRepository(SocialNetworkdDataContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<RefreshTokenEntity> GetTokenByUserIdAsync(string userId)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserID.Equals(userId));
            if (token != null)
            {
                return token;
            }

            return null;
        }
    }
}
