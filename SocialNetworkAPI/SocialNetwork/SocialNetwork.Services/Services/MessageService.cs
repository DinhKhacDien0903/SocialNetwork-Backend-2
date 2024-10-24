
using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.Services
{
    public class MessageService : IMessageService
    {
        public readonly IMessageRepository _messageRepository;

        public readonly IMessageImagesRepository _messageImageRepository;

        private readonly IMapper _mapper;
        public MessageService(
            IMessageRepository messageRepository,
            IMessageImagesRepository messageImageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _messageImageRepository = messageImageRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MessagePersonResponse>> GetAllMessagesAsync(string userId, string receiverId)
        {
            try
            {
                var messages = await _messageRepository.GetAllMessageByFriendIdAsync(userId, receiverId);

                var messagesResponse = _mapper.Map<IEnumerable<MessagePersonResponse>>(messages);

                foreach (var item in messagesResponse)
                {
                    var images = await _messageImageRepository.GetAllImageByMessageId(item.MessageID);

                    item.Images = images;
                }
                return messagesResponse;
            }
            catch (Exception e)
            {
                var x = e;
            }
            return new List<MessagePersonResponse>();
        }
    }
}
