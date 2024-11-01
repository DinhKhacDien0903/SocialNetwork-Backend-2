using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface IReactionPostService
    {
        Task AddReactionAsycn(ReactionRequest model);
        Task<IEnumerable<ReactionPostViewModel>> GetReactionByPostIdAsync(string PostId);
        Task DeleteReactionAsycn(string reactionId, string postId);
        Task UpdateReactionAsync(ReactionPostViewModel model);
    }
}
