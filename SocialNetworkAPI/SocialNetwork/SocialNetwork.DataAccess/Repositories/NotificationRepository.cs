namespace SocialNetwork.DataAccess.Repositories
{
    public class NotificationRepository : BaseRepository<NotificationEntity> , INotificationRepository
    {
        //public readonly SocialNetworkdDataContext _context;

        public NotificationRepository(SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationEntity>> GetAllNotificationMessageAsync(string userId)
        {
            var notifications = await _context.Notifications
                     .Where(n => n.ReceiverId == userId &&
                                 !n.IsRead &&
                                 !n.IsDelete &&
                                 n.IsNotificationMessage)
                     .GroupBy(n => new { n.SenderId, n.GroupId })
                     .Select(grouped => new NotificationEntity
                     {
                         SenderId = grouped.Key.SenderId,
                         GroupId = grouped.Key.GroupId,
                         ReceiverId = userId,
                         IsNotificationMessage = true,
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
