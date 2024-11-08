using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Web.Hubs
{
    public class ReactionHub : Hub
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly IReactionHubService _reactionHubService;

        private readonly IChatHubService _chatHubService;
        public ReactionHub(
            UserManager<UserEntity> userManager,
            IReactionHubService reactionHubService,
            IChatHubService chatHubService)
        {
            _userManager = userManager;
            _reactionHubService = reactionHubService;
            _chatHubService = chatHubService;
        }

        //todo => create base hub to common all hub with onconnect and disconnect
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

        private async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            await _chatHubService.UpdateStatusActiveUser(userId, isActive);
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

        public async Task<string> AddOrUpdateReactionToMessage(ReactionMessageRequest param)
        {
            var reactiondate = DateTime.UtcNow;

            var reactionId = await _reactionHubService.AddOrUpdateReaction(param);

            var reciverReactionResponse = new ReactionMessageResponse
            {
                ReactionID = reactionId,
                EmotionType = param.EmotionType,
                MessageId = param.MessageId,
                ReciverId = param.ReciverId,
                SenderId = param.SenderId,
                ReactionAt = reactiondate
            };

            await Clients.User(param.ReciverId).SendAsync("ReceiveReactionMessage", reciverReactionResponse);

            return reactionId;
        }

        public async Task RemoveReactionToMessage(ReactionMessageRequest param, string reactionId)
        {

            await _reactionHubService.RemoveReactionByReactionIdAync(reactionId);

            var reciverReactionResponse = new ReactionMessageResponse
            {
                ReactionID = reactionId,
                MessageId = param.MessageId,
                ReciverId = param.ReciverId,
                IsRemove = true
            };

            await Clients.User(param.ReciverId).SendAsync("ReceiveReactionMessage", reciverReactionResponse);
        }

    }
}
