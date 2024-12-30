
using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        private readonly IMessageImagesRepository _messageImageRepository;

        private readonly IReactionMessageRepository _reactionMessageRepository;

        private readonly IReactionRepository _reactionRepository;

        private readonly IMapper _mapper;
        public MessageService(
            IMessageRepository messageRepository,
            IMessageImagesRepository messageImageRepository,
            IReactionRepository reactionRepository,
            IReactionMessageRepository reactionMessageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _messageImageRepository = messageImageRepository;
            _reactionRepository = reactionRepository;
            _reactionMessageRepository = reactionMessageRepository;
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

                    var reactionId = await _reactionMessageRepository.GetReactionIdByMessageIdAsync(item.MessageID);

                    var reactions = await _reactionRepository.GetReactionUserByReactionIdAsync(reactionId, userId);

                    item.Images = images;

                    item.ReactionByUser = reactions;

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
