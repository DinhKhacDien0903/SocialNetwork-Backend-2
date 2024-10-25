using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface IReactionRepository
    {
        Task AddAsync(ReactionEntity entity);
        Task<ReactionEntity> GetByIdAsync(Guid reactionId);
        Task UpdateAsync(ReactionEntity entity);
        //Task DeleteAsync(Guid UserId ,Guid reactionId);
    }
}
