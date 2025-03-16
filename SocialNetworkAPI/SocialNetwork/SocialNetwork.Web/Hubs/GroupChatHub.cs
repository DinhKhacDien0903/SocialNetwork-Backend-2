using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Domain.Entities;
using SocialNetwork.DTOs.ViewModels;
namespace SocialNetwork.Web.Hubs
{
    public class GroupChatHub : Hub
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly IChatHubService _chatHubService;

        private readonly IHubContext<NotificationHub> _notificationHubContext;

        private const int MAX_MESSAGE_LENGTH = 500;

        private const string MESSAGE_NOTIFICATION = "You have a new message";
        public GroupChatHub(
            UserManager<UserEntity> userManager,
            IChatHubService chatHubService,
            IHubContext<NotificationHub> notificationHubContext)
        {
            _userManager = userManager;
            _chatHubService = chatHubService;
            _notificationHubContext = notificationHubContext;
        }

        #region'Group chat'

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await ValidateCurrentAccount();

                // Lấy groupId từ query string
                string groupId = Context.GetHttpContext()?.Request?.Query["groupId"];

                if (string.IsNullOrEmpty(groupId))
                {
                    await Clients.Caller.SendAsync("Error", "GroupId is required to connect to GroupChatHub.");
                    return;
                }

                // Thêm user vào nhóm chat
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
                Console.WriteLine($"User {user.Id} added to group {groupId}");

                // Gửi thông báo cho các thành viên trong nhóm
                await Clients.Group(groupId).SendAsync("UserConnectedInGroup", user.Id);

                await base.OnConnectedAsync();
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("Error", e.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = await ValidateCurrentAccount();

                // Lấy groupId từ query string
                string groupId = Context.GetHttpContext()?.Request?.Query["groupId"];

                if (!string.IsNullOrEmpty(groupId))
                {
                    // Xóa user khỏi nhóm
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
                    Console.WriteLine($"User {user.Id} removed from group {groupId}");

                    // Gửi thông báo cho các thành viên trong nhóm
                    await Clients.Group(groupId).SendAsync("UserDisconnectedFromGroup", user.Id);
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during disconnection: {e.Message}");
            }
        }


        public async Task<GroupChatMessageViewModel> SendMessageToGroup(SendMessageToGroupRequest param)
        {
            var sender = await ValidateCurrentAccount();

            param.Content = param.Content.Trim();

            var message = await SaveMessageToGroup(sender.Id, param);

            if (param.Images.Any())
            {
                await SaveMessageImagesGroupChat(message.GroupChatMessageID, param.Images, sender.Id);
            }

            await NotifyGroupReceiverAsync(param.GroupChatId, CreateGroupChatMessageResponse(message));

            //try
            //{
            //    var isNotificationExist = await _chatHubService.IsNotificationExist(sender.Id, param.ReciverId);

            //    if (!isNotificationExist)
            //    {
            //        var notification = await SaveNotificationToUser(sender.Id, param.ReciverId, MESSAGE_NOTIFICATION);

            //        await _notificationHubContext.Clients.User(param.ReciverId).SendAsync("ReceiveNotification", notification);
            //    }
            //}
            //catch (Exception c)
            //{

            //    var x = c.Message;
            //}
            return message;
        }

        public async Task<string> UpdateMessage(UpdateMessageRequest param)
        {
            if (!string.IsNullOrEmpty(param.MessageId) && !string.IsNullOrEmpty(param.Content))
            {
                var updateDatetime = DateTime.UtcNow;

                await _chatHubService.UpdateGroupChatMessage(param, updateDatetime);

                var response = new MessageGroupResponse
                {
                    GroupChatMessageID = param.MessageId,
                    Content = param.Content,
                    UpdatedAt = updateDatetime,
                    ReactionByUser = param.ReactionByUser
                };

                await NotifyGroupReceiverAsync(param.ReciverId, response);

            }
            return param.MessageId;
        }
        public async Task RemoveMessage(string messageId, string groupId)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                await _chatHubService.RemoveGroupChatMessage(messageId);

                var response = new MessageGroupResponse
                {
                    GroupChatMessageID = messageId,
                    IsDeleted = true
                };

                await NotifyGroupReceiverAsync(groupId, response);
            }
        }

        public async Task<string> UpdateGroupChatAvatar(UpdateGroupChatRequest param)
        {
            if (!string.IsNullOrEmpty(param.Avatar) && !string.IsNullOrEmpty(param.GroupchatId))
            {
                var updateDatetime = DateTime.UtcNow;

                await _chatHubService.UpdateGroupChatAvatar(param, updateDatetime);

                //await NotifyReceiverAsync(param.GroupchatId, param);

            }
            return param.Avatar;
        }

        private MessageGroupResponse CreateGroupChatMessageResponse(GroupChatMessageViewModel message)
        {
            return new MessageGroupResponse
            {
                UserID = message.UserID,
                GroupChatID = message.GroupChatID,
                GroupChatMessageID = message.GroupChatMessageID,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                Images = message.Images,
                Symbol = message.Symbol
            };
        }

        private async Task<GroupChatMessageViewModel> SaveMessageToGroup(string senderId, SendMessageToGroupRequest request)
        {
            var sendDatetime = DateTime.UtcNow;

            var messageViewModel = new GroupChatMessageViewModel
            {
                UserID = senderId,
                GroupChatID = request.GroupChatId,
                Content = request.Content,
                CreatedAt = sendDatetime,
                Images = request.Images,
                Symbol = request.Symbol
            };

            return await _chatHubService.AddMessageGroupAsync(messageViewModel);
        }

        private async Task NotifyGroupReceiverAsync(string groupId, MessageGroupResponse response)
        {
            await Clients.GroupExcept(groupId, new[] { Context.ConnectionId })
              .SendAsync("ReceiveSpecitificGroupChatMessage", response);
        }

        private async Task SaveMessageImagesGroupChat(string messageId, List<string> images, string senderId)
        {
            var messageImages = images.Select(image => new GroupChatMessageImageViewModel
            {
                GroupChatMessageImageID = Guid.NewGuid().ToString(),
                GroupChatMessageID = messageId,
                ImageUrl = image,
                UserID = senderId
            }).ToList();

            await _chatHubService.AddMessageImagesGroupChatAsync(messageImages);
        }

        private async Task<IdentityUser> ValidateCurrentAccount()
        {
            var x = Context.User;
            var user = await _userManager.GetUserAsync(Context.User);

            if (user == null)
            {
                await Clients.Caller.SendAsync("UserNotConnected", "You must login to chat!");

                Context.Abort();

                throw new Exception("UserNotConnected!");
            }

            return user;
        }
        #endregion
    }
}
