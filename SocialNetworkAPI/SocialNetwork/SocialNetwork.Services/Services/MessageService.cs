
using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.Services
{
    public class MessageService : IMessageService
    {
        public readonly IMessageRepository _messageRepository;

        private readonly IMapper _mapper;
        public MessageService(
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MessagePersonResponse>> GetAllMessagesAsync(string userId, string receiverId)
        {
            var messages = await _messageRepository.GetAllMessageByFriendIdAsync(userId, receiverId);

            return _mapper.Map<IEnumerable<MessagePersonResponse>>(messages);
        }
    }
}
