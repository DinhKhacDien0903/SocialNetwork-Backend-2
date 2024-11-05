using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionPostRepository 
    {
        Task<ReactionPostEntity> GetByPostIdAndUserIdAsync(string postId, string userId);
        Task AddAsync(ReactionPostEntity reactionPost);
        Task UpdateAsync(ReactionPostEntity reactionPost);
        Task DeleteAsync(ReactionPostEntity reactionPost);
        Task<IEnumerable<ReactionPostEntity>> GetAllReactionsByPostIdAsync(string postId);
    }

}
