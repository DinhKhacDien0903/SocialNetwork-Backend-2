

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

            await _messageImageRepository.SaveChangeAsync();
        }

        public async Task<string> AddMessagePersonAsync(MessageViewModel messageViewModel)
        {
            var entity = _mapper.Map<MessagesEntity>(messageViewModel);

            entity.UpdatedAt = entity.CreatedAt;

            entity.MessageID = Guid.NewGuid().ToString();

            var message =  await _messageRepository.AddAsync(entity);

            var messageID = message.MessageID;

            await _messageRepository.SaveChangeAsync();

            return messageID;
        }

        public async Task RemoveMessage(string messageId)
        {
            (await _messageRepository.GetByIDAsync(messageId)).IsDeleted = true;

            await _messageRepository.SaveChangeAsync();
        }

        public async Task UpdateMessage(UpdateMessageRequest param, DateTime updateDatetime)
        {
            var currentMessage = await _messageRepository.GetByIDAsync(param.MessageId);

            currentMessage.Content = param.Content;

            currentMessage.UpdatedAt = updateDatetime;

            await _messageRepository.SaveChangeAsync();
        }

        public async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            await _userRepository.UpdateStatusActiveUser(userId, isActive);
        }
    }
}
