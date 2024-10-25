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

        public async Task<string> SendMessageToPerson(SendMessageToPersonRequest param)
        {
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(param.ReciverId);
            
            var sendDate = DateTime.UtcNow.AddHours(7);

            param.Content = param.Content.Trim();

            if(reciver == null)
            {
                await Clients.Caller.SendAsync("UserNotConnected", "User not found!");

                return "";
            }

            if (string.IsNullOrEmpty(param.Content) && param.Images.Count == 0)
            {
                await Clients.Caller.SendAsync("MessageValidateError", "Message cannot be empty");
                return "";
            }

            if (param.Content.Length > 500)
            {
                await Clients.Caller.SendAsync("MessageValidateTooLarge", "Message is too long. Max is 500 characters");
                return "";
            }

            var messageViewModel = new MessageViewModel
            {
                SenderID = sender.Id,
                ReciverID = reciver.Id,
                Content = param.Content,
                CreatedAt = sendDate,
                Images = param.Images
            };

            var messageID = await _chatHubService.AddMessagePersonAsync(messageViewModel);

            if (param.Images.Count > 0)
            {
                var listImageViewModel = new List<MessageImageViewModel>();

                foreach (var image in param.Images)
                {
                    listImageViewModel.Add(
                        new MessageImageViewModel
                        {
                            MessageImageID = Guid.NewGuid().ToString(),
                            MessageID = messageID,
                            ImageUrl = image
                        }
                    );
                }

                await _chatHubService.AddMessageImagesAsync(listImageViewModel);
            }

            var messageResponse = new MessagePersonResponse
            {
                MessageID = messageID,
                Content = param.Content,
                Images = param.Images,
                SendDate = sendDate,
                Symbol = param.Symbol
            }; 

            await Clients.User(reciver.Id).SendAsync("ReceiveSpecitificMessage", messageResponse);

            return messageID;
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

    }
}
