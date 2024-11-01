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

namespace SocialNetwork.Services.Services
{
    

    public class PostHubService : IPostHubService
    {
        private readonly IHubContext<PostHub> _hubContext;

        public PostHubService(IHubContext<PostHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task SendPostAsycn(PostRequest postViewModel)
        {
            await _hubContext.Clients.All.SendAsync("ReceivePost", postViewModel);
        }

        public async Task SendUpdateAsycn(PostRequest updateViewModel)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdatePost", updateViewModel);
        }

        public async Task SendDeleteAsycn(Guid Id)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDeletePost", Id);
        }
    }
}


