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

        public async Task<string> SendMessageToPerson(string recevierId, string message, int symbol)
        {
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(recevierId);
            
            var sendDate = DateTime.UtcNow.AddHours(7);

            message = message.Trim();

            if(reciver == null)
            {
                await Clients.Caller.SendAsync("UserNotConnected", "User not found!");

                return "";
            }

            if (string.IsNullOrEmpty(message))
            {
                await Clients.Caller.SendAsync("MessageValidateError", "Message cannot be empty");
                return "";
            }

            if (message.Length > 500)
            {
                await Clients.Caller.SendAsync("MessageValidateTooLarge", "Message is too long. Max is 500 characters");
                return "";
            }

            var messageViewModel = new MessageViewModel
            {
                SenderID = sender.Id,
                ReciverID = reciver.Id,
                Content = message,
                CreatedAt = sendDate
            };

            var messageID = await _chatHubService.AddMessagePerson(messageViewModel);

            var recevierMessageViewModel = new RecevierMessageViewModel
            {
                MessageID = messageID,
                SenderID = sender.Id,
                RecevierID = reciver.Id,
                Message = message,
                SendDate = sendDate
            };

            await Clients.User(reciver.Id).SendAsync("ReceiveSpecitificMessage", messageID, message, sendDate);

            return messageID;
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

    }
}
