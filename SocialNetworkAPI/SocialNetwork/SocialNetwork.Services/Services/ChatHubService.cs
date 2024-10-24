

using SocialNetwork.DataAccess.Repositories;

namespace SocialNetwork.Services.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageImagesRepository _messageImageRepository;
        private readonly IMapper _mapper;
        public ChatHubService(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMessageImagesRepository messageImagesRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _messageImageRepository = messageImagesRepository;
            _mapper = mapper;
        }

        public async Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages)
        {
            var listEntites = _mapper.Map<IEnumerable<MessageImageEntity>>(messageImages);

            await _messageImageRepository.AddRangeAsync(listEntites);
        }

        public async Task<string> AddMessagePersonAsync(MessageViewModel messageViewModel)
        {
            var entity = _mapper.Map<MessagesEntity>(messageViewModel);

            var message =  await _messageRepository.AddAsync(entity);

            var messageID = message.MessageID;

            await _messageRepository.SaveChangeAsync();

            return messageID;
        }

        public async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            await _userRepository.UpdateStatusActiveUser(userId, isActive);
        }
    }
}
