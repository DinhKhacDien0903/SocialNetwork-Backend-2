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
        
        public async Task SendUpdatePostAsycn(PostViewModel post)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveUpdatePost", post);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sendUpdatePost");

            }
        }

        public async Task SendDelete(Guid id)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveDeletePost", id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sendDelete");
            }
        }

        //public async Task SendReaction(ReactionRequest reactionRequest)
        //{
        //    await Clients.All.SendAsync("ReceiveReaction", reactionRequest);
        //}

        public async Task RemoveReaction(string postId, string userId)
        {
            await Clients.All.SendAsync("ReceiveRemoveReaction", postId, userId);
        }



        public async Task StartPostRoom(string postId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, postId);
        }

        public async Task LeavePostRoom(string postId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId);
        }

        public async Task SendCommentAsync(CommentViewModel comment)
        {
            try
            {
                // Gửi bình luận đến tất cả các client trong phòng của bài đăng
                await Clients.Group(comment.PostID.ToString()).SendAsync("ReceiveComment", comment);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SendCommentAsync", ex);
            }
        }

        public async Task SendNotification(string usedId, NotificationViewModel message)
        {
            await Clients.User(usedId).SendAsync("ReceiveNotification", message);
        }

    }
}
