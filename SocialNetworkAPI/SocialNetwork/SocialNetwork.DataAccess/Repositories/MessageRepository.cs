using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.DataAccess.Repositories
{
    public class MessageRepository : BaseRepository<MessagesEntity>, IMessageRepository
    {
        public readonly SocialNetworkdDataContext _context;

        private readonly UserManager<UserEntity> _userManager;
        public MessageRepository(
            SocialNetworkdDataContext context,
            UserManager<UserEntity> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<MessagesEntity>> GetAllMessageByFriendIdAsync(string userId, string receiverId)
        {
      
            return await _context.Messages.Where(
            x => (x.SenderID.Equals(userId) && x.ReciverID.Equals(receiverId)) ||
                    (x.SenderID.Equals(receiverId) && x.ReciverID.Equals(userId)) &&
                    !x.IsDeleted).OrderBy(x => x.CreatedAt).ToListAsync();

        }
    }
}
