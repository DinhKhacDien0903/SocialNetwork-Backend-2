using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionPostRepository : BaseRepository<ReactionPostEntity>, IReactionPostRepository
    {
        private readonly SocialNetworkdDataContext _context;

        public ReactionPostRepository(SocialNetworkdDataContext context) : base(context)
        {

            _context = context;
        }


        public async Task AddReactionAsync(ReactionPostEntity reactionPost)
        {
            await _context.ReactionPosts.AddAsync(reactionPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReactionAsync(Guid reactionId,Guid postId)
        {
            //var reactionPost = await _context.ReactionPosts.Include(x=>x.Reaction).FirstOrDefaultAsync(x => x.PostID == postId && x.ReactionID == reactionId);
            var reactionPost = await _context.ReactionPosts.Include(x=>x.Reaction).Include(x=>x.Post).FirstOrDefaultAsync(x => x.PostID == postId && x.ReactionID == reactionId);
            if (reactionPost != null)
            {
                var reactionEntity = reactionPost.Reaction;
                if (reactionEntity != null)
                {
                    reactionEntity.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
            }
        }

      

        //public async Task<ReactionPostEntity> GetReactionByIdAsync(Guid reactionId)
        //{
        //    var reaction=await _context.ReactionPosts.FindAsync(reactionId);
        //    return reaction;
        //}

        public async Task<IEnumerable<ReactionPostEntity>> GetReactionsByPostIdAsync(Guid postId)
        {
            var reaction = await _context.ReactionPosts.Include(x => x.Reaction).Where(x => x.PostID == postId && x.Reaction.IsDeleted == false).ToListAsync();
            return reaction;
        }

        public async Task UpdateReactionAsync(ReactionPostEntity reactionPost)
        {
            _context.ReactionPosts.Update(reactionPost);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserHasReactionAsync(string userId, Guid PostId)
        {
            return await _context.ReactionPosts.AnyAsync(x => x.PostID == PostId && x.Reaction.UserID == userId);
        }
    }
}
