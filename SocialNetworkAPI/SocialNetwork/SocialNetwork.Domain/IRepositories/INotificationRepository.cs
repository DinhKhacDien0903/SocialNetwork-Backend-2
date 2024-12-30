using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Domain.IRepositories
{
    public interface INotificationRepository : IBaseRepository<NotificationEntity>
    {
        Task<IEnumerable<NotificationEntity>> GetAllNotificationMessageAsync(string userId);
        Task<NotificationEntity> UpdateNotificationMessageAsync(string userId, string? friendId, string? groupId);
        Task AddSendFriendAsync(NotificationEntity entity);
        public Task AcceptNotificationAsync(NotificationEntity notification);
        Task<IEnumerable<NotificationEntity>> GetAllFriendRequest(string userId);
        Task<NotificationEntity> FirstOrIdNotification(string id);

        Task<NotificationEntity> AddOrUpdateAsync(NotificationEntity entity);

        Task<bool> IsNotificationExist(string senderId, string reciverId, string groupId = null);

        Task<string> FindNotificationId(string senderid, string receiverid);
    }
}
