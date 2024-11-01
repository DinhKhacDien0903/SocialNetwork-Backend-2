using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface IPostHubService
    {

        Task SendPostAsycn(PostRequest postViewModel);

        Task SendUpdateAsycn(PostRequest updateViewModel);

        Task SendDeleteAsycn(Guid Id);


    }
}
