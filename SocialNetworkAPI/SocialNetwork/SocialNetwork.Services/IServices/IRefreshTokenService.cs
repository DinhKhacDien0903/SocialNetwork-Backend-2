namespace SocialNetwork.Services.IServices
{
    public interface IRefreshTokenService
    {
        Task CreateRefreshTokenAsync(RefreshTokenEntity entity);

        Task<RefreshTokenEntity> GetRefreshTokeByTokenAsync(string token);

        Task UpdateRefreshTokenAsync(string token);
    }
}
