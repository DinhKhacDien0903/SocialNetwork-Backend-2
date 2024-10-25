using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly SocialNetworkdDataContext _context;

        public ReactionRepository(SocialNetworkdDataContext context)
        {

            _context = context;
        }
        public async Task AddAsync(ReactionEntity entity)
        {
            await _context.Reactions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ReactionEntity> GetByIdAsync(Guid id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            return reaction;
        }

        public async Task UpdateAsync(ReactionEntity entity)
        {
            _context.Reactions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
