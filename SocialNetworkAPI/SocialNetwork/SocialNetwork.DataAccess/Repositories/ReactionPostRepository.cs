using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ReactionPostRepository : IReactionBaseRepository<ReactionPostEntity, ReactionPostEntity>
    {
        private readonly SocialNetworkdDataContext _context;

        public ReactionPostRepository(SocialNetworkdDataContext context) 
        {
            _context = context;
        }


        public async Task AddAsync(ReactionPostEntity reactionPost)
        {
            await _context.ReactionPosts.AddAsync(reactionPost);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ReactionPostEntity reactionPost)
        {
            _context.ReactionPosts.Update(reactionPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ReactionPostEntity reactionPost)
        {
            reactionPost.Post = await _context.Posts.FindAsync(reactionPost.PostID);
            reactionPost.Reaction = await _context.Reactions.FindAsync(reactionPost.ReactionID);
            reactionPost.Reaction.IsDeleted = true;
            _context.ReactionPosts.Update(reactionPost);
            await _context.SaveChangesAsync();
        }

        public async Task<ReactionEntity> GetReactionByReactionIdAsync(string reactionId)
        {
            var reaction= await _context.Reactions
                                 .Where(r => r.ReactionID == reactionId&& !r.IsDeleted)
                                 .FirstOrDefaultAsync();

            return reaction;
        }



        public async Task<IEnumerable<ReactionPostEntity>> GetAllReactionsIdAsync(string postId)
        {
            var reaction = await _context.ReactionPosts.Where(x => x.PostID == postId && !x.Reaction.IsDeleted).ToListAsync();
            return reaction;    

        }


            public async Task<ReactionPostEntity> GetIdAndUserIdAsync(string postId,string userId)
        {
            var post=await _context.Posts.FindAsync(postId);
            //var reaction=await _context.Reactions.FindAsync()
            var reaction = await _context.ReactionPosts.
                Where(x => x.PostID == postId && x.Reaction.UserID==userId && x.Post==post&& !x.Reaction.IsDeleted)
                .FirstOrDefaultAsync();




            return reaction;
        }

        

        //public async Task<bool> UserHasReactionAsync(string userId, string PostId)
        //{
        //    return await _context.ReactionPosts.AnyAsync(x => x.PostID == PostId && x.Reaction.UserID == userId);
        //}
    }
}
