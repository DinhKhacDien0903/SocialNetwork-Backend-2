using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.DataAccess.Repositories
{
    public class RelationshipRepository : BaseRepository<RelationshipEntity>, IRelationshipRepository
    {
        public readonly SocialNetworkdDataContext _context;

        public RelationshipRepository(SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _context.Relationships.Where(x => x.UserID == userId).Select(x => x.FriendID).ToListAsync();
        }
    }
}
