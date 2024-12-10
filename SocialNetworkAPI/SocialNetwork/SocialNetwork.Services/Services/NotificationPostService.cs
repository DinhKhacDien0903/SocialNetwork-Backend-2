using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Services
{
    public class NotificationPostService : INotificationPostService
    {
        public readonly INotificationPostRepository _notification;
        public readonly IMapper _mapper;

        public NotificationPostService(IMapper mapper, INotificationPostRepository notification)
        {
            _mapper = mapper;
            _notification = notification;
        }

        public async Task CreateNotificationAsync(NotificationPostViewModel model, List<string> friendId)
        {
            var notifications = friendId.Select(friendId => new NotificationPostEntity
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

        public async Task<IEnumerable<NotificationPostViewModel>> GetUserNotificationAsync(string userId)
        {
            
            var notification= await _notification.GetNotificationByUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationPostViewModel>>(notification);
        }

        public async Task MarkNotificationAsync(string id)
        {
            await _notification.MakeAsReadAsync(id);
        }
    }
}
