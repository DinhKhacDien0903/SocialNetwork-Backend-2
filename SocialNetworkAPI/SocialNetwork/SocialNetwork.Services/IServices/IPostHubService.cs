using SocialNetwork.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface IPostHubService
    {

        Task SendPostAsync(PostResponse postViewModel);

        Task SendUpdateAsycn(PostRequest updateViewModel);

        Task SendDeleteAsycn(Guid Id);

        Task SendCommentAsycn(CommentRespone commentRespone);
    }
}
