using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public  interface INotificationPostRepository
    {
        public Task CreateNotificationAsync(IEnumerable<NotificationPostEntity> notification);
        public Task<IEnumerable<NotificationPostEntity>> GetNotificationByUserAsync(string userId);
        public Task MakeAsReadAsync(string id);
        public Task DeleteNotificationAsync(int id);
    }
}
