using SocialNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task AcceptNotificationAsync(NotificationRequestFriendViewModel notification)
        {
            var notificationEntity = _mapper.Map<NotificationEntity>(notification);
            notificationEntity.Id = Guid.NewGuid().ToString();
            notificationEntity.Type = 2;
            await _notificationRepository.AcceptNotificationAsync(notificationEntity);
        }

        public async Task AddNotificationAsync(NotificationRequestFriendViewModel model)
        {
            
           var notificationEntity=  _mapper.Map<NotificationEntity>(model);
            notificationEntity.Id=Guid.NewGuid().ToString();
            notificationEntity.Type = 1;
            await _notificationRepository.AddSendFriendAsync(notificationEntity);
        }

        public Task CreateNotificationAsync(NotificationViewModel notificationViewModel, List<string> friendId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FriendRequestViewmodel>> GetAllFriendRequest(string userId)
        {
            var allRequest = await _notificationRepository.GetAllFriendRequest(userId);
            var friendsRequest = allRequest
                 .Where(x =>  x.Sender != null)
                .Select(x => new FriendRequestViewmodel()
            {
                LastName = x.Sender.LastName,
                FirstName = x.Sender.FirstName,
                AvatarUrl = x.Sender.AvatarUrl,
                Message = x.Messeage,
                Type=x.Type,
                Id = x.Id,
                SenderId=x.SenderId,
            }).ToList();
            return friendsRequest;
           
        }

        public Task<IEnumerable<NotificationViewModel>> GetUserNotificationAsync(string userId)
        {
            throw new NotImplementedException();

        }

        public Task MarkNotificationAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
