using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    //public class ReactionCommentRepositories : IReactionBaseRepository<ReactionCommentEntity, ReactionCommentEntity>
    //{
    //    private readonly SocialNetworkdDataContext _context;

    //    public ReactionCommentRepositories(SocialNetworkdDataContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task AddAsync(ReactionCommentEntity reaction)
    //    {
    //        await _context.ReactionComments.AddAsync(reaction); 
    //        await _context.SaveChangesAsync();

    //    }

    //    public async Task DeleteAsync(ReactionCommentEntity reaction)
    //    {
    //        reaction.Reaction.IsDeleted = true; 
    //         _context.ReactionComments.Update(reaction);  
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task<IEnumerable<ReactionCommentEntity>> GetAllReactionsIdAsync(string commentId)
    //    {
    //        var reaction=await _context.ReactionComments.Where(x=>x.CommentID==commentId && !x.Reaction.IsDeleted).ToListAsync();
    //        return reaction;
    //    }

    //    public Task<ReactionCommentEntity> GetIdAndUserIdAsync(string postId, string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<ReactionEntity> GetReactionByReactionIdAsync(string reactionId)
    //    {
    //        var reaction = await _context.Reactions.Where(x => x.ReactionID == reactionId && 
    //        !x.IsDeleted).FirstOrDefaultAsync();
    //        return reaction;
    //    }

    //    public async Task UpdateAsync(ReactionCommentEntity reaction)
    //    {
    //        _context.ReactionComments.Update(reaction);
    //        await _context.SaveChangesAsync();
    //    }
    //}
}
