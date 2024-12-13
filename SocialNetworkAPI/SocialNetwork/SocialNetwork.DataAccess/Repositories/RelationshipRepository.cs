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

        public async Task AccepFriendRequestAsync(string userId, string friendId)
        {
            var accUser= await _context.Relationships.FindAsync(userId,friendId);
            var accFriend= await _context.Relationships.FindAsync(friendId,userId);
            if (accUser != null)
            {
                accUser.Status= FriendshipStatus.Accepted;
                accFriend.Status= FriendshipStatus.Accepted;
               await _context.SaveChangesAsync();    
            }
        }

        public async Task CancelFriendAsync(string userId, string friendId)
        {
            var friend=await _context.Relationships.FindAsync(userId, friendId);
            if (friend != null)
            {
                friend.Status = FriendshipStatus.Canceled;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeclineFriendRequestAsync(string userId, string friendId)
        {
            var friend=await _context.Relationships.FindAsync(userId, friendId);
            if (friend != null)
            {
                friend.Status = FriendshipStatus.Declined;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeclineFriendAsync(string userId, string friendId)
        {
            var friend = await _context.Relationships.FindAsync(friendId, userId);
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
            return await _context.Relationships.Where(x => x.UserID == userId&& !x.IsDeleted && x.Status== FriendshipStatus.Accepted).Select(x => x.FriendID).ToListAsync();/*|| x.FriendID == userId*/
        }

        public async Task<IEnumerable<UserEntity>> GetPendingFriendRequestAsync(string userId)
        {
            var listSendFriend= await _context.Relationships.Where(x=>x.FriendID==userId && x.Status==FriendshipStatus.Pending).Select(x => x.User).ToListAsync();
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

        public async Task<IEnumerable<UserEntity>> GetSendFriendRequestAsync(string userId)
        {
            var listSend=await _context.Relationships.Where(x=>x.UserID==userId&& x.Status== FriendshipStatus.Pending).Include(x=>x.Friend).ToListAsync();
            var sendFriend = listSend.Select(x => new UserEntity
            {
                FirstName = x.Friend.FirstName,
                LastName = x.Friend.LastName,
                AvatarUrl = x.Friend.AvatarUrl,
                Id=x.Friend.Id,
                
            });
            return sendFriend;
        }

        
    }
}
