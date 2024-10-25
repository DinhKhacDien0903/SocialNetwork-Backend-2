using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionPostRepository : IBaseRepository<ReactionPostEntity>
    {
        Task AddReactionAsync(ReactionPostEntity reactionPost);
        Task<IEnumerable<ReactionPostEntity>> GetReactionsByPostIdAsync(Guid postId);
        Task UpdateReactionAsync(ReactionPostEntity reactionPost);
        Task DeleteReactionAsync(Guid reactionId,Guid postId);
        Task<bool> UserHasReactionAsync(string userId, Guid postId);
        //Task<ReactionPostEntity> GetReactionByIdAsync(Guid reactionId);
    }

}
