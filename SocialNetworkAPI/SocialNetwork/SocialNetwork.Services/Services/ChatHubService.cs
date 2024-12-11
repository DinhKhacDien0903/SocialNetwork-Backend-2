using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Services.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageImagesRepository _messageImageRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public ChatHubService(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMessageImagesRepository messageImagesRepository,
            INotificationRepository notificationRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _messageImageRepository = messageImagesRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages)
        {
            var listEntites = _mapper.Map<IEnumerable<MessageImageEntity>>(messageImages);

            await _messageImageRepository.AddRangeAsync(listEntites);

            await _messageImageRepository.SaveChangeAsync();
        }

        public async Task<MessageViewModel> AddMessagePersonAsync(MessageViewModel messageViewModel)
        {
            var entity = _mapper.Map<MessagesEntity>(messageViewModel);

            entity.UpdatedAt = entity.CreatedAt;

            entity.MessageID = Guid.NewGuid().ToString();

            var message =  await _messageRepository.AddAsync(entity);

            messageViewModel.MessageID = message.MessageID;

            await _messageRepository.SaveChangeAsync();

            return messageViewModel;
        }

        public async Task<NotificationViewModel> AddNotificationToUserAsync(NotificationViewModel notificationViewModel)
        {
            try
            {
                var entity = _mapper.Map<NotificationEntity>(notificationViewModel);

                entity.Id = Guid.NewGuid().ToString();

                var notification = await _notificationRepository.AddAsync(entity);

                await _notificationRepository.SaveChangeAsync();

                notificationViewModel.Id = notification.Id;

                return notificationViewModel;
            }catch(Exception e)
            {
                var x = e.Message;
                return new NotificationViewModel();
            }
        }

        public async Task<IEnumerable<NotificationViewModel>> GetAllNotificationMessageAsync(string userId)
        {
            var notification = await _notificationRepository.GetAllNotificationMessageAsync(userId);

            return _mapper.Map<IEnumerable<NotificationViewModel>>(notification);
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
