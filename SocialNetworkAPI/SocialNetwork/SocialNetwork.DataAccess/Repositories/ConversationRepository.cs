using Microsoft.AspNetCore.Identity;
using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.Response;

namespace SocialNetwork.DataAccess.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        public readonly SocialNetworkdDataContext _context;
        public ConversationRepository(
            SocialNetworkdDataContext context)
        {
            _context = context;
        }

        public async Task<BaseSearchConversationResponse> GetAllConversationAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount)
        {
            try
            {
                var result = new BaseSearchConversationResponse();

                //Get all friends of current user util unfriend
                var friends = from rl in _context.Relationships
                              where rl.UserID == userId && !rl.IsDeleted
                              select rl.FriendID;

                //Get all group chat of current user unitl leave
                var groups = from g in _context.GroupChatMembers
                             where g.UserID == userId && !g.IsLeaved
                             select g.GroupChatID;

                var combinedConversations = (from u in _context.Users
                                             where friends.Contains(u.Id) &&
                                                   (string.IsNullOrEmpty(searchText) ||
                                                    string.Concat(u.LastName.ToLower(), u.FirstName.ToLower()).Contains(searchText.ToLower()))
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
                                                 LastMessageCreated = lastMessage.CreatedAt,
                                                 GroupId = string.Empty,
                                                 GroupName = string.Empty,
                                                 AdministratorId = string.Empty,
                                                 IsGroupChat = false
                                             })

                                             .Union(
                                                 from g in _context.GroupChats
                                                 where groups.Contains(g.GroupChatID) &&
                                                       (string.IsNullOrEmpty(searchText) ||
                                                        g.GroupName.ToLower().Contains(searchText.ToLower()))
                                                 let lastMessage = _context.GroupChatMessages
                                                     .Where(m => m.GroupChatID == g.GroupChatID)
                                                     .OrderByDescending(m => m.CreatedAt)
                                                     .FirstOrDefault()
                                                 select new LatestConversationsResponse
                                                 {
                                                     SenderId = lastMessage.UserID ?? userId,
                                                     FriendId = string.Empty,
                                                     FirstName = string.Empty,
                                                     LastName = string.Empty,
                                                     Avatar = g.Avatar ?? "",
                                                     Message = lastMessage.Content ?? "Nhóm mới được tạo",
                                                     LastMessageCreated = lastMessage.CreatedAt,
                                                     GroupId = g.GroupChatID,
                                                     GroupName = g.GroupName,
                                                     AdministratorId = userId,
                                                     IsGroupChat = true
                                                 }
                                             )
                                             .OrderByDescending(c => c.LastMessageCreated);

                var total = combinedConversations.Count();

                if (isTotalCount)
                {
                    result.TotalPage = total / pageSize;
                    result.TotalCount = total;

                    if (total % pageSize != 0)
                    {
                        result.TotalPage++;
                    }
                }
                else
                {
                    result.Conversations = await combinedConversations
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseSearchFriendRespone> GetFriendsAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount)
        {
            try
            {
                var result = new BaseSearchFriendRespone();

                //Get all friends of current user util unfriend
                var friends = from rl in _context.Relationships
                              join u in _context.Users on rl.FriendID equals u.Id
                              where rl.UserID == userId &&
                              !rl.IsDeleted &&
                              (string.IsNullOrEmpty(searchText) || string.Concat(u.LastName.ToLower(), u.FirstName.ToLower()).Contains(searchText.ToLower()))
                              select new FriendResponse
                              {
                                  Id = rl.FriendID,
                                  FirstName = u.FirstName ?? "",
                                  LastName = u.LastName ?? "",
                                  AvatarUrl = u.AvatarUrl ?? "",
                              };

                var total = friends.Count();

                if (isTotalCount)
                {
                    result.TotalPage = total / pageSize;
                    result.TotalCount = total;

                    if (total % pageSize != 0)
                    {
                        result.TotalPage++;
                    }
                }
                else
                {
                    result.Friends = await friends
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseSearchGroupChatsRespone> GetGroupChatsAsync(string userId, string searchText, int pageIndex, int pageSize, bool isTotalCount)
        {
            try
            {
                var result = new BaseSearchGroupChatsRespone();

                //Get all group chat of current group util leaved
                var groups = from gc in _context.GroupChats
                             join gcm in _context.GroupChatMembers on gc.GroupChatID equals gcm.GroupChatID
                             where gcm.UserID == userId &&
                             !gcm.IsLeaved &&
                             (string.IsNullOrEmpty(searchText) || gc.GroupName.ToLower().Contains(searchText.ToLower()))
                             select new GroupResponse
                             {
                                 Id = gc.GroupChatID,
                                 Name = gc.GroupName ?? "",
                                 AvatarUrl = gc.Avatar ?? "",
                             };

                var total = groups.Count();

                if (isTotalCount)
                {
                    result.TotalPage = total / pageSize;
                    result.TotalCount = total;

                    if (total % pageSize != 0)
                    {
                        result.TotalPage++;
                    }
                }
                else
                {
                    result.Groups = await groups
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseSearchGroupMemberResponse> GetGroupMembersAsync(string userId, string searchText, string GroupID, int pageIndex, int pageSize, bool isTotalCount)
        {
            try
            {
                var result = new BaseSearchGroupMemberResponse();

                //Get all group chat member of current group util leaved
                var groupMembers = from u in _context.Users
                                   join gcm in _context.GroupChatMembers on u.Id equals gcm.UserID
                                   where /*gcm.UserID == userId &&*/
                                         gcm.GroupChatID == GroupID &&
                                         !gcm.IsLeaved &&
                                         //todo isUserDelete
                                         (string.IsNullOrEmpty(searchText) || string.Concat(u.LastName.ToLower(), u.FirstName.ToLower()).Contains(searchText.ToLower()))
                                   select new GroupMemberResponse
                                   {
                                       Id = u.Id,
                                       FirstName = u.FirstName ?? "",
                                       LastName = u.LastName ?? "",
                                       AvatarUrl = u.AvatarUrl ?? "",
                                   };

                var total = groupMembers.Count();

                if (isTotalCount)
                {
                    result.TotalPage = total / pageSize;
                    result.TotalCount = total;

                    if (total % pageSize != 0)
                    {
                        result.TotalPage++;
                    }
                }
                else
                {
                    result.GroupMembers = await groupMembers
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
