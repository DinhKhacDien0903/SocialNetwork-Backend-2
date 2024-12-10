using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface INotificationPostService
    {
        public Task<IEnumerable<NotificationPostViewModel>> GetUserNotificationAsync(string userId);
        public Task CreateNotificationAsync(NotificationPostViewModel notificationViewModel, List<string> friendId);
        public Task MarkNotificationAsync(string id);

    }
}
