namespace SocialNetwork.Domain.IRepositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshTokenEntity>
    {
        Task<RefreshTokenEntity> GetTokenByUserIdAsync(string userId);
    }
}
