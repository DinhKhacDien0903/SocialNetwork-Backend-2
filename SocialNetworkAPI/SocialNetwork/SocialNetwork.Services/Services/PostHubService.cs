using Microsoft.AspNetCore.SignalR;
using SocialNetwork.DataAccess;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SocialNetwork.Helpers.Hubs;
using SocialNetwork.DTOs.Response;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Services.IServices;

namespace SocialNetwork.Services.Services
{
    

    public class PostHubService : IPostHubService
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUserService _userService;


        public PostHubService(IHubContext<PostHub> hubContext, UserManager<UserEntity> userManager, IUserService userService)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _userService = userService;
        }


        public async Task SendPostAsync(PostResponse post)
        {
            await _hubContext.Clients.All.SendAsync("ReceivePost", post);
           
        }

        public async Task SendUpdateAsycn(PostRequest updateViewModel)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdatePost", updateViewModel);
        }

        public async Task SendCommentAsycn(CommentViewModel commentRespone,int number)
        {
            var result = new
            {
                comment = commentRespone,
                number = number
            };
            await _hubContext.Clients.All.SendAsync("ReceiveComment", result);

        }
        public async Task SendReactionAddAsycn(ReactionRequest reactionRequest)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveReaction", reactionRequest);
        }
        public async Task SendReactionUpdateAsycn(ReactionRequest reactionRequest)
        {
            await _hubContext.Clients.All.SendAsync("updateEmotion", reactionRequest);
        }

        public async Task SendDeleteReactionAsycn(string userId, string postId)
        {
            await _hubContext.Clients.All.SendAsync("cancelReleasedEmotion", new
            {
                PostID=postId,
                UserID=userId
            });

        }
      

        public async Task SendFriendRequestNotification(FriendRequestViewmodel model,string friendid)
        {
            await _hubContext.Clients.User(friendid).SendAsync("FriendRequestNotification",model);
        }

        public async Task SendRefusePostAsync(string postId)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveRefusePost", postId);
        }

        public async Task AcceptFriendNotification(FriendRequestViewmodel model, string friendid)
        {
            await _hubContext.Clients.User(friendid).SendAsync("AcceptFriend", model);

        }

        public async Task CancelFriend( string userId, string friendid)
        {
            await _hubContext.Clients.User(userId).SendAsync("CancelUser",friendid);
            await _hubContext.Clients.User(friendid).SendAsync("CancelUser", userId);
        }

        public async Task SendGetNotification(List<FriendRequestViewmodel> model)
        {
            await _hubContext.Clients.All.SendAsync("getNotification", model);
        }

        public async Task SendSeedSearch()
        {
            var users = await _userService.GetAllUsersAsync();

           
            await _hubContext.Clients.All.SendAsync("seedSearUser", users);
        }
    }
}


