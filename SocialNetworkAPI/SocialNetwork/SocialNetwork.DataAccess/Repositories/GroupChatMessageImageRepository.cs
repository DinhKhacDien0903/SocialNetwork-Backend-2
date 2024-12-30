
namespace SocialNetwork.DataAccess.Repositories
{
    public class GroupChatMessageImageRepository : BaseRepository<GroupChatMessageImageEntity>, IGroupChatMessageImageRepository
    {
        public readonly SocialNetworkdDataContext _context;

        public GroupChatMessageImageRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllImageByMessageId(string MessageId)
        {
            return await _context.GroupChatMessageImages
                .Where(x => x.GroupChatMessageID.ToString().Equals(MessageId) && !x.IsDeleted)
                .Select(x => x.ImageUrl)
                .ToListAsync();
        }
    }
}
