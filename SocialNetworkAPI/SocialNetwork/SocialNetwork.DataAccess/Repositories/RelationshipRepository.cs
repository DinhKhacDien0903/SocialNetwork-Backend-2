using Azure.Core;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain;

namespace SocialNetwork.DataAccess.Repositories
{
    public class RelationshipRepository : BaseRepository<RequestFriendEntity>, IRelationshipRepository
    {
        public readonly SocialNetworkdDataContext _context;

        private readonly INotificationRepository _notificationRepository;


        public RelationshipRepository(SocialNetworkdDataContext context, INotificationRepository notificationRepository) : base(context)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task AccepFriendRequestAsync(string userId, string friendId)
         {
            try
            {
                var accUser = await _context.RequestFriends.FirstOrDefaultAsync(r => r.UserID == friendId && r.FriendID == userId);
                var accFriend = await _context.RequestFriends.FirstOrDefaultAsync(r => r.UserID == userId && r.FriendID == friendId);

                if (accUser == null && accFriend == null)
                {
                    throw new Exception("Friend request not found");
                }

                var relationship1 = new RelationshipEntity
                {
                    UserID = userId,
                    FriendID = friendId,
                    IsDeleted = false,
                    DeletedAt = null
                };
                var relationship2 = new RelationshipEntity
                {
                    UserID = friendId,
                    FriendID = userId,
                    IsDeleted = false,
                    DeletedAt = null
                };

                // Thêm vào bảng Relationships
                await _context.Relationships.AddAsync(relationship1);
                await _context.Relationships.AddAsync(relationship2);

                // Tìm notification nếu có
                var notificationUser = await _notificationRepository.FirstOrIdNotification(friendId);
                if (notificationUser != null)
                {
                    _context.Notifications.Remove(notificationUser);
                }

                if (accFriend != null)
                {
                    _context.RequestFriends.Remove(accFriend);
                }
                if (accUser != null)
                {
                    _context.RequestFriends.Remove(accUser);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in AcceptFriendRequestAsync: {ex.Message}", ex);
            }
        }

        public async Task CancelFriendAsync(string userId, string friendId)
        {
            var friend1 = await _context.Relationships.FirstOrDefaultAsync(r => r.UserID == userId && r.FriendID == friendId);
            var friend2 = await _context.Relationships.FirstOrDefaultAsync(r => r.UserID == friendId && r.FriendID == userId);
            if (friend1 != null && friend2 != null)
            {
                _context.Relationships.Remove(friend1);
                _context.Relationships.Remove(friend2);
                await _context.SaveChangesAsync();
            }
            var notificationUser = await _notificationRepository.FirstOrIdNotification(friendId);
            if (notificationUser != null)
            {
                _context.Notifications.Remove(notificationUser);
            }
        }

        public async Task DeclineFriendRequestAsync(string userId, string friendId)
        {
            var friend = await _context.RequestFriends.FirstOrDefaultAsync(r => r.UserID == userId && r.FriendID == friendId);
            if (friend != null)
            {
                var notificationUser = await _notificationRepository.FirstOrIdNotification(userId);
                //_context.Notifications.Remove(notificationUser);
                if (notificationUser != null)
                {
                    _context.Notifications.Remove(notificationUser);
                }
                //friend.Status = FriendshipStatus.Declined;
                _context.RequestFriends.Remove(friend);
                await _context.SaveChangesAsync();
            }
           
        }
        public async Task DeclineFriendAsync(string userId, string friendId)
        {
            var friend = await _context.RequestFriends.FirstOrDefaultAsync(r => (r.UserID == friendId && r.FriendID == userId) 
            || (r.UserID == userId && r.FriendID == friendId));

            var notificationUser = await _notificationRepository.FirstOrIdNotification(friendId);
            if (notificationUser != null)
            {
                var userSendId = notificationUser.SenderId;
                _context.Notifications.Remove(notificationUser);
                var userSend = await _context.RequestFriends.FirstOrDefaultAsync(r => r.UserID == userSendId && r.FriendID == userId);

                if (friend == null && userSend == null)
                {
                    throw new Exception("not");
                }
                if (friend == null)
                {
                    _context.RequestFriends.Remove(userSend);
                    //userSend.Status = FriendshipStatus.Declined;
                    await _context.SaveChangesAsync();
                }
            }

            _context.RequestFriends.Remove(friend);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId)
        {
            var listFriend = await _context.Relationships.Where(x => x.UserID == userId)
                .Select(x => x.Friend).ToListAsync();
            return listFriend;

        }

        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _context.Relationships.Where(x => x.UserID == userId).Select(x => x.FriendID).ToListAsync();/*|| x.FriendID == userId*/
        }

        public async Task<IEnumerable<UserEntity>> GetPendingFriendRequestAsync(string userId)
        {
            var listSendFriend = await _context.RequestFriends.Where(x => x.FriendID == userId && x.Status == FriendshipStatus.Pending).Select(x => x.User).ToListAsync();
            return listSendFriend;
        }



        public async Task<string> SendFriendRequestAsync(string userId, string friendId)
        {
            var requestExit = await _context.RequestFriends.AnyAsync(x => x.UserID == userId && x.FriendID == friendId && x.Status == FriendshipStatus.Pending);
            var relationShipExit = await _context.Relationships.AnyAsync(x => x.UserID == userId && x.FriendID == friendId);
            if (requestExit || relationShipExit)
            {
                return "Not";
            }
            var friend = new RequestFriendEntity
            {
                UserID = userId,
                FriendID = friendId,
                Status = FriendshipStatus.Pending,
            };
            _context.RequestFriends.Add(friend);
            await _context.SaveChangesAsync();
            return "Ok";
        }

        public async Task<IEnumerable<UserEntity>> GetSendFriendRequestAsync(string userId)
        {
            var listSend = await _context.RequestFriends.Where(x => x.UserID == userId && x.Status == FriendshipStatus.Pending).Include(x => x.Friend).ToListAsync();
            var sendFriend = listSend.Select(x => new UserEntity
            {
                FirstName = x.Friend.FirstName,
                LastName = x.Friend.LastName,
                AvatarUrl = x.Friend.AvatarUrl,
                Id = x.Friend.Id,

            });
            return sendFriend;
        }

        public Task AddRelationShipAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
    }
}
