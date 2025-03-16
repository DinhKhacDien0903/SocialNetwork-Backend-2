using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.DataAccess.Repositories
{
    public class GroupChatMessageRepository : BaseRepository<GroupChatMessageEntity>, IGroupChatMessageRepository
    {
        public readonly SocialNetworkdDataContext _context;

        public GroupChatMessageRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupChatMessageEntity>> GetAllMessageByGroupIdAsync(string groupId)
        {
            return await _context.GroupChatMessages.Where(
            x => (x.GroupChatID.Equals(groupId) &&
                 !x.IsDeleted)).OrderBy(x => x.UpdatedAt).ToListAsync();
        }
    }
}
