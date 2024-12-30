using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace SocialNetwork.DataAccess.Repositories
{
    public class NotificationRepository : BaseRepository<NotificationEntity> , INotificationRepository
    {
        public readonly SocialNetworkdDataContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public NotificationRepository(SocialNetworkdDataContext context, UserManager<UserEntity> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AcceptNotificationAsync(NotificationEntity entity)
        {
            bool notificationExit = await _context.Notifications.AnyAsync(x => x.ReceiverId == entity.ReceiverId && x.SenderId == entity.SenderId);
            if (!notificationExit)
            {
                await _context.Notifications.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddSendFriendAsync(NotificationEntity entity)
        {
            try
            {
                bool notificationExit = await _context.Notifications.AnyAsync(x => x.ReceiverId == entity.ReceiverId && x.SenderId == entity.SenderId);
                if (!notificationExit)
                {
                     await _context.Notifications.AddAsync(entity);
                await _context.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {

                throw new Exception("fail");
            }
        }

        public async Task<NotificationEntity> FirstOrIdNotification(string id)
        {
            var notificationUser= await _context.Notifications.FirstOrDefaultAsync(x=>x.SenderId == id);
            return notificationUser;
        }

        public async Task<IEnumerable<NotificationEntity>> GetAllFriendRequest(string userId)
        {
            //var user=await _userManager.FindByIdAsync(userId);
            var allRequest = await _context.Notifications.Where(x => x.ReceiverId == userId&&x.Type!=0).ToListAsync();

            foreach (var item in allRequest)
            {
                if (item.Sender == null)
                {
                   var sender=await _userManager.FindByIdAsync(item.SenderId);
                   
                    item.Sender=sender;
                }
            }
            return allRequest;
        }
        public async Task<IEnumerable<NotificationEntity>> GetAllNotificationMessageAsync(string userId)
        {
            var notifications = await _context.Notifications
                     .Where(n => n.ReceiverId == userId &&
                                 !n.IsRead &&
                                 !n.IsDelete &&
                                 n.Type == 0)
                     .GroupBy(n => new { n.SenderId, n.GroupId })
                     .Select(grouped => new NotificationEntity
                     {
                         SenderId = grouped.Key.SenderId,
                         GroupId = grouped.Key.GroupId,
                         ReceiverId = userId,
                         Type = 0,
                         Id = grouped.OrderByDescending(x => x.CreatedAt).First().Id,
                         Messeage = grouped.OrderByDescending(x => x.CreatedAt).First().Messeage,
                         CreatedAt = grouped.Max(x => x.CreatedAt),
                         UpdatedAt = grouped.OrderByDescending(x => x.CreatedAt).First().UpdatedAt,
                         IsRead = grouped.OrderByDescending(x => x.CreatedAt).First().IsRead,
                     })
                     .ToListAsync();

            return notifications;
        }
    }
}
