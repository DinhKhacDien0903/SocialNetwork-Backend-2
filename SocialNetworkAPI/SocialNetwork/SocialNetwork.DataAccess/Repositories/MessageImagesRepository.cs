
namespace SocialNetwork.DataAccess.Repositories
{
    public class MessageImagesRepository : BaseRepository<MessageImageEntity>, IMessageImagesRepository
    {
        private readonly SocialNetworkdDataContext _context;
        public MessageImagesRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllImageByMessageId(string MessageId)
        {
            return await _context.MessageImages
                .Where(x => x.MessageID.ToString().Equals(MessageId) && !x.IsDeleted)
                .Select(x => x.ImageUrl)
                .ToListAsync();
        }
    }
}
