using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface INotificationService
    {
        public Task<IEnumerable<NotificationViewModel>> GetUserNotificationAsync(string userId);
        public Task CreateNotificationAsync(NotificationViewModel notificationViewModel, List<string> friendId);
        public Task MarkNotificationAsync(string id);
        Task<IEnumerable<FriendRequestViewmodel>> GetAllFriendRequest(string userId);
        public Task AddNotificationAsync(NotificationRequestFriendViewModel notification);
        public Task AcceptNotificationAsync(NotificationRequestFriendViewModel notification);
    }
}
