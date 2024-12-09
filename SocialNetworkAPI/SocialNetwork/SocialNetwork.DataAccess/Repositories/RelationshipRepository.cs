using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain;

namespace SocialNetwork.DataAccess.Repositories
{
    public class RelationshipRepository : BaseRepository<RelationshipEntity>, IRelationshipRepository
    {
        public readonly SocialNetworkdDataContext _context;

        public RelationshipRepository(SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task AccepFriendRequestAsync(string friendId)
        {
            var friend= await _context.Relationships.FindAsync(friendId);
            if (friend != null)
            {
                friend.Status= FriendshipStatus.Accepted;
               await _context.SaveChangesAsync();    
            }
        }

        public async Task CancelFriendRequestAsync(string friendId)
        {
            var friend=await _context.Relationships.FindAsync((friendId));
            if (friend != null)
            {
                friend.Status = FriendshipStatus.Canceled;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeclineFriendRequestAsync(string friendId)
        {
            var friend=await _context.Relationships.FindAsync(friendId);
            if (friend != null)
            {
                friend.Status = FriendshipStatus.Declined;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId)
        {
            var listFriend = await _context.Relationships.Where(x => x.UserID ==userId&&x.Status==FriendshipStatus.Accepted)
                .Select(x=>x.Friend).ToListAsync();
            return listFriend;

        }

        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _context.Relationships.Where(x => x.UserID == userId&& !x.IsDeleted).Select(x => x.FriendID).ToListAsync();/*|| x.FriendID == userId*/
        }

        public async Task<IEnumerable<RelationshipEntity>> GetPendingFriendRequestAsync(string userId)
        {
            var listSendFriend= await _context.Relationships.Where(x=>x.UserID==userId && x.Status==FriendshipStatus.Pending).ToListAsync();
            return listSendFriend;  
        }

        public async Task SendFriendRequestAsync(string userId, string friendId)
        {
            var friend = new RelationshipEntity
            {
                UserID = userId,
                FriendID = friendId,
                Status = FriendshipStatus.Pending,
            };
            _context.Relationships.Add(friend);
            await _context.SaveChangesAsync();
        }
    }
}
