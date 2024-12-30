using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<UserEntity> _userManager;  

        private readonly IChatHubService _chatHubService;

        private readonly IHubContext<NotificationHub> _notificationHubContext;

        private const int MAX_MESSAGE_LENGTH = 500;

        private const string MESSAGE_NOTIFICATION = "You have a new message";
        public ChatHub(
            UserManager<UserEntity> userManager,
            IChatHubService chatHubService,
            IHubContext<NotificationHub> notificationHubContext)
        {
            _userManager = userManager;
            _chatHubService = chatHubService;
            _notificationHubContext = notificationHubContext;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await ValidateCurrentAccount();

                await Groups.AddToGroupAsync(Context.ConnectionId, user.Id);

                await UpdateStatusActiveUser(user.Id, true);

                await Clients.Others.SendAsync("UserConnected", user.Id);


                await base.OnConnectedAsync();
            }
            catch
            (Exception e)
            {
                await Clients.Caller.SendAsync("UserNotConnected", e.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await ValidateCurrentAccount();

            await UpdateStatusActiveUser(user.Id, false);
                
            await Clients.Others.SendAsync("UserDisConnected", user.Id);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<MessageViewModel> SendMessageToPerson(SendMessageToPersonRequest param)
        {
            
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(param.ReciverId);

            //var curentMessage = await _chatHubService.IsMessageExist(param.)

            param.Content = param.Content.Trim();

            if ( !await ValidateMessage(param, reciver.Id))
            {
                return MessageViewModel.Empty;
            }

            var message = await SaveMessage(sender.Id, param);

            if (param.Images.Any())
            {
                await SaveMessageImages(message.MessageID, param.Images);
            }

            await NotifyReceiverAsync(param.ReciverId, CreateMessageResponse(message, param));

            try
            {
                var notifiation = await SaveNotificationToUser(sender.Id, param.ReciverId, MESSAGE_NOTIFICATION);

                await _notificationHubContext.Clients.User(param.ReciverId).SendAsync("ReceiveNotification", notifiation);
            }
            catch(Exception c)
            {

                var x = c.Message;
            }
            return message;
        }

        public async Task OnUserTyping(string reciverId)
        {
            await Clients.User(reciverId).SendAsync("ReciverTypingNotification", true);
        }

        public async Task StoppedUserTyping(string reciverId)
        {
            await Clients.User(reciverId).SendAsync("ReciverTypingNotification", false);
        }

        private async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            await _chatHubService.UpdateStatusActiveUser(userId, isActive);
        }

        public async Task RemoveMessage(string messageId, string reciverId)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                await _chatHubService.RemoveMessage(messageId);

                var response = new MessagePersonResponse
                {
                    MessageID = messageId,
                    IsDelete = true
                };

                await NotifyReceiverAsync(reciverId, response);
            }
        }


        public async Task<string> UpdateMessage(UpdateMessageRequest param)
        {
            if(!string.IsNullOrEmpty(param.MessageId) && !string.IsNullOrEmpty(param.Content))
            {
                var updateDatetime = DateTime.UtcNow;

                await _chatHubService.UpdateMessage(param, updateDatetime);

                var response = new MessagePersonResponse
                {
                    MessageID = param.MessageId,
                    Content = param.Content,
                    UpdateAt = updateDatetime,
                    ReactionByUser = param.ReactionByUser
                };

                await NotifyReceiverAsync(param.ReciverId, response);

            }
            return param.MessageId;
        }

        private async Task<IdentityUser> ValidateCurrentAccount()
        {
            var x  = Context.User;
            var user = await _userManager.GetUserAsync(Context.User);

            if(user == null)
            {
                await Clients.Caller.SendAsync("UserNotConnected", "You must login to chat!");

                Context.Abort();

                throw new Exception("UserNotConnected!");
            }

            return user;
        }

        private async Task<bool> ValidateMessage(SendMessageToPersonRequest request, string receiver)
        {
            request.Content = request.Content?.Trim();

            if (IsEmptyMessage(request))
            {
                await Clients.Caller.SendAsync("MessageValidateError", "Message cannot be empty");
                return false;
            }

            if (IsMessageTooLong(request.Content))
            {
                await Clients.Caller.SendAsync("MessageValidateTooLarge", $"Message is too long. Max is {MAX_MESSAGE_LENGTH} characters");
                return false;
            }

            return true;
        }

        private bool IsEmptyMessage(SendMessageToPersonRequest request)
        {
            return string.IsNullOrEmpty(request.Content)
                && !request.Images.Any()
                && request.Symbol == 0;
        }

        private bool IsMessageTooLong(string content)
        {
            return content?.Length > MAX_MESSAGE_LENGTH;
        }

        private async Task<MessageViewModel> SaveMessage(string senderId, SendMessageToPersonRequest request)
        {
            var sendDatetime = DateTime.UtcNow;

            var messageViewModel = new MessageViewModel
            {
                SenderID = senderId,
                ReciverID = request.ReciverId,
                Content = request.Content,
                CreatedAt = sendDatetime,
                Images = request.Images,
                Symbol = request.Symbol
            };

            return await _chatHubService.AddMessagePersonAsync(messageViewModel);
        }

        private async Task SaveMessageImages(string messageId, List<string> images)
        {
            var messageImages = images.Select(image => new MessageImageViewModel
            {
                MessageImageID = Guid.NewGuid().ToString(),
                MessageID = messageId,
                ImageUrl = image
            }).ToList();

            await _chatHubService.AddMessageImagesAsync(messageImages);
        }

        private MessagePersonResponse CreateMessageResponse(MessageViewModel message, SendMessageToPersonRequest request)
        {
            return new MessagePersonResponse
            {
                SenderID = message.SenderID,
                MessageID = message.MessageID,
                Content = request.Content,
                Images = request.Images,
                CreatedAt = message.CreatedAt,
                Symbol = request.Symbol
            };
        }

        private async Task NotifyReceiverAsync(string receiverId, MessagePersonResponse response)
        {
            await Clients.User(receiverId).SendAsync("ReceiveSpecitificMessage", response);
        }

        private async Task<NotificationViewModel> SaveNotificationToUser(string senderId, string friendId, string message)
        {
            var sendDatetime = DateTime.UtcNow;

            var notificationViewModel = new NotificationViewModel
            {
                SenderId = senderId,
                ReceiverId = friendId,
                Messeage = message,
                CreatedAt = sendDatetime,
                UpdatedAt = sendDatetime,
                Type = 0
            };

            return await _chatHubService.AddNotificationToUserAsync(notificationViewModel);
        }
    }
}
