using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.Response;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Helpers.Hubs
{
    public class PostHub:Hub
    {

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendPostAsync(PostResponse post)
        {
            try
                {
                await Clients.All.SendAsync("ReceivePost", post);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sendPost");
            }
        }
        public async Task SendRefusePostAsync(string id)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveRefusePost", id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sendDelete");
            }
        }
        public async Task RemoveReaction(string postId, string userId)
        {
            await Clients.All.SendAsync("ReceiveRemoveReaction", postId, userId);
        }
        public async Task SendCommentAsync(object comment)
        {
            try
            {
                //await Clients.Group(comment.PostID.ToString()).SendAsync("ReceiveComment", comment);
                await Clients.All.SendAsync("ReceiveComment", comment);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SendCommentAsync", ex);
            }
        }
        public async Task SendFriendRequest(FriendRequestViewmodel model, string friendId)
        {
            await Clients.User(friendId).SendAsync("FriendRequestNotification" ,model);
        }
        public async Task SendAcceptRequest(FriendRequestViewmodel model, string friendId)
        {
            await Clients.User(friendId).SendAsync("AcceptFriend", model);
        }
        public async Task SendSearchAsync()
        {
            await Clients.All.SendAsync("SearchUser");
        }
        public async Task SendGetNotification(List<FriendRequestViewmodel> model)
        {
            await Clients.All.SendAsync("GetNotification", model);
        }
        public async Task CancelFriend(string userId, string friendid)
        {
            await Clients.User(userId).SendAsync("CancelUser", friendid);
            await Clients.User(friendid).SendAsync("CancelUser", userId);
        }
        public async Task SendSeedSearch()
        {
            await Clients.All.SendAsync("CancelUser");
        }
    }
}
