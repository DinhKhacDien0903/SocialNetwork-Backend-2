using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Domain.IRepositories
{
    public interface INotificationRepository : IBaseRepository<NotificationEntity>
    {
        Task<IEnumerable<NotificationEntity>> GetAllNotificationMessageAsync(string userId);
        Task AddSendFriendAsync(NotificationEntity entity);
        public Task AcceptNotificationAsync(NotificationEntity notification);


        Task<IEnumerable<NotificationEntity>> GetAllFriendRequest(string userId);
        Task<NotificationEntity> FirstOrIdNotification(string id);
    }
}
