using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Entities;
using System.Text.RegularExpressions;
namespace SocialNetwork.Web.Hubs
{
    public class ReactionGroupChatMessageHub : Hub
    {

        private readonly UserManager<UserEntity> _userManager;

        private readonly IReactionHubService _reactionHubService;

        private readonly IChatHubService _chatHubService;
        public ReactionGroupChatMessageHub(
            UserManager<UserEntity> userManager,
            IReactionHubService reactionHubService,
            IChatHubService chatHubService)
        {
            _userManager = userManager;
            _reactionHubService = reactionHubService;
            _chatHubService = chatHubService;
        }

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

        public async Task<string> AddOrUpdateReactionToMessage(ReactionMessageRequest param)
        {
            var reactiondate = DateTime.UtcNow;

            var reactionId = await _reactionHubService.AddOrUpdateReactionGroupChatMessage(param);

            var reciverReactionResponse = new ReactionMessageResponse
            {
                ReactionID = reactionId,
                EmotionType = param.EmotionType,
                MessageId = param.MessageId,
                GroupId = param.GroupId,
                SenderId = param.SenderId,
                ReactionAt = reactiondate
            };

            //await Clients.User(param.ReciverId).SendAsync("ReceiveReactionMessage", reciverReactionResponse);
            await Clients.GroupExcept(param.GroupId, new[] { Context.ConnectionId })
              .SendAsync("ReceiveReactionMessage", reciverReactionResponse);
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
    }

}
