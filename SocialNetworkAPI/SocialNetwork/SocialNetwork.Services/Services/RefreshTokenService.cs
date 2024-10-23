using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.DataContext;

namespace SocialNetwork.Services.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly SocialNetworkdDataContext _context;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, SocialNetworkdDataContext context)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _context = context;
        }

        public async Task CreateRefreshTokenAsync(RefreshTokenEntity entity)
        {
            await  _refreshTokenRepository.AddAsync(entity);

            await _refreshTokenRepository.SaveChangeAsync();
        }

        public async Task<RefreshTokenEntity> GetRefreshTokeByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task UpdateRefreshTokenAsync(string token)
        {
            try
            {


                var refreshToken = await GetRefreshTokeByTokenAsync(token);

                refreshToken.IsUsed = true;
                refreshToken.IsRevoked = true;

                _refreshTokenRepository.Update(refreshToken);

                await _refreshTokenRepository.SaveChangeAsync();
            }catch(Exception e)
            {
                throw new Exception();
            }
        }
    }
}
