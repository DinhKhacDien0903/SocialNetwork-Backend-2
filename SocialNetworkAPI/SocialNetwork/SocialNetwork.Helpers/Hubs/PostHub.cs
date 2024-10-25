using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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

        public async Task SendPostAsync(PostViewModel post)
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
                await Clients.All.SendAsync("ReceiveUpdatePost", id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sendDelete");

            }
        }


    }
}
