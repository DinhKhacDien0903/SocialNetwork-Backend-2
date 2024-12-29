

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Services.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IPostHubService _postHubService;
        public RelationshipService(IRelationshipRepository relationshipRepository, IMapper mapper, INotificationService notificationService, UserManager<UserEntity> userManager, IPostHubService postHubService)
        {
            _relationshipRepository = relationshipRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _userManager = userManager;
            _postHubService = postHubService;
        }

       

       

        public async Task<IEnumerable<UserSearchViewModel>> GetAllFriendAsync(string userId)
        {

            var getFriend=await _relationshipRepository.GetAllFriendAsync(userId);
            var friend=  _mapper.Map<IEnumerable<UserSearchViewModel>>(getFriend);
            return friend;
        }

        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _relationshipRepository.GetFriendIdByUserId(userId);
        }

        public async Task<IEnumerable<UserSearchViewModel>> GetPendingFriendRequestAsync(string userId)
        {
            var getFriendPeding = await _relationshipRepository.GetPendingFriendRequestAsync(userId);
            var friend = _mapper.Map<IEnumerable<UserSearchViewModel>>(getFriendPeding);
            return friend;
        }

        public async Task<IEnumerable<UserSearchViewModel>> GetSendFriendRequestAsync(string userId)
        {
            var model = await _relationshipRepository.GetSendFriendRequestAsync(userId);
            var friend= _mapper.Map<IEnumerable<UserSearchViewModel>>(model);
            return friend;
        }

        public  async Task<FriendRequestViewmodel> SendFriendRequest(string userId, string friendId)
        {
            
            var getFriend = await  _relationshipRepository.SendFriendRequestAsync(userId, friendId);
           

            if (getFriend == "Ok")
            {
                var userinf = await _userManager.FindByIdAsync(userId);

                //var user = await _userManager.FindByIdAsync(userId);
                string message = $"{userinf.LastName} {userinf.FirstName} đã gửi lời mời kết bạn";
                var usersend = new FriendRequestViewmodel
                {
                    LastName = userinf.LastName,
                    FirstName = userinf.FirstName,
                    AvatarUrl = userinf.AvatarUrl,
                    Id = userinf.Id,
                    Type=1,
                    Message = message,
                    SenderId=friendId,
                };
                var notification = new NotificationRequestFriendViewModel
                {
                    SenderId = userId,
                    ReceiverId = friendId,
                    Messeage = message,
                    //Type=1,
                };
                await _notificationService.AddNotificationAsync(notification);

                await _postHubService.SendFriendRequestNotification(usersend,friendId);
                
                return usersend;
            }
            else
            {
                throw new Exception("Không thể gửi lời mời kết bạn. Vui lòng thử lại sau.");
            }
        }
        public   async Task AccepFriendRequestAsync(string userId, string friendId)
        { 
            var userinf = await _userManager.FindByIdAsync(userId);
            if (userinf == null)
            {
                throw new ArgumentException("Người dùng không tồn tại.", nameof(userId));
            }

             await _relationshipRepository.AccepFriendRequestAsync(userId, friendId);

            string message = $"{userinf.LastName} {userinf.FirstName} đã chấp nhận mời kết bạn";
            var usersend = new FriendRequestViewmodel
            {
                LastName = userinf.LastName,
                FirstName = userinf.FirstName,
                AvatarUrl = userinf.AvatarUrl,
                Id = userinf.Id,
                Type = 2,
                Message = message,
                SenderId = friendId,
            };
            var notification = new NotificationRequestFriendViewModel
            {
                SenderId = userId,
                ReceiverId = friendId,
                Messeage = message,
                //Type=1,
            };
            //await _notificationService.GetAllFriendRequest(userId);
            await _notificationService.AcceptNotificationAsync(notification);
            await _postHubService.AcceptFriendNotification(usersend, friendId);
            await _postHubService.CancelFriend(userId, friendId);       
        }

        public Task CancelFriendAsync(string userId, string friendId)
        {
            var friend = _relationshipRepository.CancelFriendAsync(userId, friendId);
            return friend;

        }

        public  Task DeclineFriendAsync(string userId, string friendId)
        {
            var model = _relationshipRepository.DeclineFriendAsync(userId, friendId);
             _postHubService.CancelFriend(userId, friendId);

            return model;
        }

        public  Task DeclineFriendRequestAsync(string userId, string friendId)
        {
            var friend = _relationshipRepository.DeclineFriendRequestAsync(userId, friendId);
             _postHubService.CancelFriend(userId, friendId);
            return friend;
        }
    }
}
