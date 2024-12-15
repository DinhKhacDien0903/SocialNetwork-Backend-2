using SocialNetwork.DTOs.Request;
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

        Task SendCommentAsycn(CommentViewModel commentRespone,int number);

        Task SendReactionAddAsycn(ReactionRequest reactionRequest);

        Task SendReactionUpdateAsycn(ReactionRequest reactionRequest);

        Task SendDeleteReactionAsycn(string userId, string postId);

        Task SendNotificationToMultipleUsers(List<string> userIds, NotificationPostViewModel notification);

        Task SendRefusePostAsync(string postId);
    }
}
