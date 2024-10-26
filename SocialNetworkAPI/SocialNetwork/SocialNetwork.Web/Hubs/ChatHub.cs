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
        private const int MAX_MESSAGE_LENGTH = 500;
        public ChatHub(
            UserManager<UserEntity> userManager,
            IChatHubService chatHubService)
        {
            _userManager = userManager;
            _chatHubService = chatHubService;
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

        public async Task<string> SendMessageToPerson(SendMessageToPersonRequest param)
        {
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(param.ReciverId);

            param.Content = param.Content.Trim();

            if ( !await ValidateMessage(param, reciver.Id))
            {
                return string.Empty;
            }

            var messageId = await SaveMessage(sender.Id, param);

            if (param.Images.Any())
            {
                await SaveMessageImages(messageId, param.Images);
            }

            await NotifyReceiver(param.ReciverId, CreateMessageResponse(messageId, param));

            return messageId;
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

        private async Task<string> SaveMessage(string senderId, SendMessageToPersonRequest request)
        {
            var messageViewModel = new MessageViewModel
            {
                SenderID = senderId,
                ReciverID = request.ReciverId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow.AddHours(7),
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

        private MessagePersonResponse CreateMessageResponse(string messageId, SendMessageToPersonRequest request)
        {
            return new MessagePersonResponse
            {
                MessageID = messageId,
                Content = request.Content,
                Images = request.Images,
                SendDate = DateTime.UtcNow.AddHours(7),
                Symbol = request.Symbol
            };
        }

        private async Task NotifyReceiver(string receiverId, MessagePersonResponse response)
        {
            await Clients.User(receiverId).SendAsync("ReceiveSpecitificMessage", response);
        }
    }
}
