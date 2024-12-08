namespace SocialNetwork.Domain.IRepositories
{
    public interface INotificationRepository : IBaseRepository<NotificationEntity>
    {
        Task<IEnumerable<NotificationEntity>> GetAllNotificationMessageAsync(string userId);
    }
}
