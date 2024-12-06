using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Services
{
    public class NotificationService : INotificationService
    {
        public readonly INotificationRepository _notification;
        public readonly IMapper _mapper;

        public NotificationService(IMapper mapper, INotificationRepository notification)
        {
            _mapper = mapper;
            _notification = notification;
        }

        public async Task CreateNotificationAsync(NotificationViewModel model, List<string> friendId)
        {
            var notifications = friendId.Select(friendId => new NotificationEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = model.UserId,
                ReceiverId = friendId,
                Content = model.Content,
                Type = "New_Post",
                CreatedAt = DateTime.UtcNow
            }).ToList();
            await _notification.CreateNotificationAsync(notifications);
        }

        public async Task<IEnumerable<NotificationViewModel>> GetUserNotificationAsync(string userId)
        {
            
            var notification= await _notification.GetNotificationByUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationViewModel>>(notification);
        }

        public async Task MarkNotificationAsync(string id)
        {
            await _notification.MakeAsReadAsync(id);
        }
    }
}
