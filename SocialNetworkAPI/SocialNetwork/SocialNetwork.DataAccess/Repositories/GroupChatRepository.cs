namespace SocialNetwork.DataAccess.Repositories
{
    public class GroupChatRepository : BaseRepository<GroupChatEntity>, IGroupChatRepository
    {
        public readonly SocialNetworkdDataContext _context;
        public GroupChatRepository(
            SocialNetworkdDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GroupChatEntity> CreateGroupChatAsync(string userId, GroupChatEntity groupChat, List<string> members)
        {
            try
            {
                var entity = await AddAsync(groupChat);

                await AddGroupChatMembers(userId, entity.GroupChatID, members);

                await SaveChangeAsync();

                return entity;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                throw new Exception(ex.Message);
            }
        }

        public bool IsGroupNameExist(string groupName)
        {
            var x = _context.GroupChats.Any(x => x.GroupName.ToLower() == groupName.ToLower()); 

            return _context.GroupChats.Any(x => x.GroupName.ToLower() == groupName.ToLower());
        }
        
        public async Task<int> AddGroupChatMembers(string userId, string groupChatId, List<string> memberIds)
        {
            var listMember = new List<GroupChatMemberEntity>();

            foreach (var memberId in memberIds)
            {
                listMember.Add(new GroupChatMemberEntity
                {
                    GroupChatID = groupChatId,
                    UserID = memberId,
                    JoinAt = DateTime.UtcNow,
                    IsAdmin = userId == memberId
                });
            }

            await _context.GroupChatMembers.AddRangeAsync(listMember);

            return memberIds.Count;
        }
    }
}
