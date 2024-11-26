using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionBaseRepository<TEntity, Tkey>
    {
        Task<TEntity> GetIdAndUserIdAsync(string postId, string userId);
        Task AddAsync(Tkey reactionPost);
        Task UpdateAsync(Tkey reactionPost);
        Task DeleteAsync(Tkey reactionPost);
        Task<IEnumerable<TEntity>> GetAllReactionsIdAsync(string postId);
        Task<ReactionEntity> GetReactionByReactionIdAsync(string reactionId);
    }

}
