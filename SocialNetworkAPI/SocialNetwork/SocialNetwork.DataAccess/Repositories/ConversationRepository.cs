using Microsoft.AspNetCore.Identity;
using SocialNetwork.DTOs.Response;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        public readonly SocialNetworkdDataContext _context;

        private readonly UserManager<UserEntity> _userManager;
        public ConversationRepository(
            SocialNetworkdDataContext context,
            UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<LatestConversationsResponse>> GetAllConversationAsync(string userId, string textSearch, int skip, int take)
        {
            try
            {
                var friends = from rl in _context.Relationships
                              where rl.UserID == userId
                              select rl.FriendID;

                var groups = from g in _context.GroupChatMembers
                             where g.UserID == userId
                             select g.GroupChatID;

                // Get all friends of user
                var conversations = await  (from u in _context.Users
                                    where friends.Contains(u.Id)
                                    let lastMessage = _context.Messages
                                       .Where(m => (m.SenderID == userId && m.ReciverID == u.Id) || (m.SenderID == u.Id && m.ReciverID == userId))
                                       .OrderByDescending(m => m.CreatedAt) 
                                       .FirstOrDefault() 
                                    select new LatestConversationsResponse
                                    {
                                        SenderId = lastMessage.SenderID,
                                        FriendId = u.Id,
                                        FirstName = u.FirstName ?? "Dien",
                                        LastName = u.LastName ?? "Dinh",
                                        Avatar = u.AvatarUrl ?? "",
                                        Message = lastMessage.Content, 
                                    }).ToListAsync();

                //Get all group chat of user

                var groupChat = await (from g in _context.GroupChats
                                       where groups.Contains(g.GroupChatID)
                                       let lastMessage = _context.GroupChatMessages
                                          .Where(m => m.GroupChatID == g.GroupChatID)
                                          .OrderByDescending(m => m.CreatedAt)
                                          .FirstOrDefault()
                                       select new LatestConversationsResponse
                                       {
                                           SenderId = lastMessage.UserID ?? userId,
                                           GroupId = g.GroupChatID,
                                           GroupName = g.GroupName,
                                           Avatar = g.Avatar ?? "",
                                           Message = lastMessage.Content ?? "Nhóm mới được tạo",
                                       }).ToListAsync();

                conversations.AddRange(groupChat);

                return conversations;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
