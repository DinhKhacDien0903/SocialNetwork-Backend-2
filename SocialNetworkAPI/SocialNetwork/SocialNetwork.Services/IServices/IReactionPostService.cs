using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.IServices
{
    public interface IReactionPostService
    {
        Task<ReactionRequest> AddReactionAsync(string postId, string userId, string emotionTypeId);
        Task<bool> RemoveReactionAsync(string postId, string userId);
        Task<IEnumerable<ReactionPostEntity>> GetAllReactionsByPostIdAsync(string postId);
        Task<IEnumerable<EmotionTypeEntity>> GetAllEmotionTypesAsync();
    }
}
