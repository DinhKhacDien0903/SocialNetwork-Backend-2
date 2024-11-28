
namespace SocialNetwork.Services.Services
{
    public class GroupChatService : IGroupChatService
    {
        private readonly IGroupChatRepository _groupChatRepository;

        private readonly IMapper _mapper;

        public GroupChatService(
            IGroupChatRepository groupChatRepository,
            IMapper mapper)
        {
            _groupChatRepository = groupChatRepository;
            _mapper = mapper;
        }

        public async Task<GroupChatViewModel> CreateGroupChatAsync(string userId, GroupChatViewModel request)
        {
            var time = DateTime.UtcNow;

            var entity = _mapper.Map<GroupChatEntity>(request);

            entity.GroupChatID = Guid.NewGuid().ToString();

            entity.CreatedAt = time;

            entity.UpdatedAt = time;

            entity.Avatar = request.Avatar ?? "https://res.cloudinary.com/dlran3qvj/image/upload/v1732701622/file_1732701619587.jpg";

            var groupChat = await _groupChatRepository.CreateGroupChatAsync(userId, entity, request.Members);

            //todo custom response group chat 
            return _mapper.Map<GroupChatViewModel>(groupChat);
        }

        public bool ValidateGroupChat(GroupChatViewModel groupChat)
        {
            return _groupChatRepository.IsGroupNameExist(groupChat.GroupName);
        }
    }
}
