namespace SocialNetwork.Services.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IGroupChatMessageRepository _groupChatMessageRepository;
        private readonly IGroupChatMessageImageRepository _groupChatMessageImageRepository;
        private readonly IMessageImagesRepository _messageImageRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IMapper _mapper;
        public ChatHubService(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IGroupChatMessageRepository groupChatMessageRepository,
            IGroupChatMessageImageRepository groupChatMessageImageRepository,
            IMessageImagesRepository messageImagesRepository,
            INotificationRepository notificationRepository,
            IGroupChatRepository groupChatRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _messageImageRepository = messageImagesRepository;
            _notificationRepository = notificationRepository;
            _groupChatMessageRepository = groupChatMessageRepository;
            _groupChatMessageImageRepository = groupChatMessageImageRepository;
            _groupChatRepository = groupChatRepository;
            _mapper = mapper;
        }

        public async Task<GroupChatMessageViewModel> AddMessageGroupAsync(GroupChatMessageViewModel messageViewModel)
        {
            var entity = _mapper.Map<GroupChatMessageEntity>(messageViewModel);

            entity.UpdatedAt = entity.CreatedAt;

            entity.GroupChatMessageID = Guid.NewGuid().ToString();

            var message = await _groupChatMessageRepository.AddAsync(entity);

            messageViewModel.GroupChatMessageID = message.GroupChatMessageID;

            await _groupChatMessageRepository.SaveChangeAsync();

            return messageViewModel;
        }

        public async Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages)
        {
            var listEntites = _mapper.Map<IEnumerable<MessageImageEntity>>(messageImages);

            await _messageImageRepository.AddRangeAsync(listEntites);

            await _messageImageRepository.SaveChangeAsync();
        }

        public async Task AddMessageImagesGroupChatAsync(List<GroupChatMessageImageViewModel> messageImages)
        {
            var listEntites = _mapper.Map<IEnumerable<GroupChatMessageImageEntity>>(messageImages);

            await _groupChatMessageImageRepository.AddRangeAsync(listEntites);

            await _groupChatMessageImageRepository.SaveChangeAsync();
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

                var notification = await  _notificationRepository.AddOrUpdateAsync(entity);

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

        public Task<bool> IsNotificationExist(string senderId, string reciverId)
        {
            return _notificationRepository.IsNotificationExist(senderId, reciverId);
        }

        public async Task<NotificationViewModel> ReadMessageNotificationAsync(string userId, string friendId, string groupId)
        {
            var notification = await _notificationRepository.UpdateNotificationMessageAsync(userId, friendId, groupId);

            return _mapper.Map<NotificationViewModel>(notification);
        }

        public async Task RemoveGroupChatMessage(string messageId)
        {
            (await _groupChatMessageRepository.GetByIDAsync(messageId)).IsDeleted = true;

            await _groupChatMessageRepository.SaveChangeAsync();
        }

        public async Task RemoveMessage(string messageId)
        {
            (await _messageRepository.GetByIDAsync(messageId)).IsDeleted = true;

            await _messageRepository.SaveChangeAsync();
        }

        public async Task UpdateGroupChatAvatar(UpdateGroupChatRequest param, DateTime updateDatetime)
        {
            await _groupChatRepository.UpdateGroupChatAvatar(param.GroupchatId, param.Avatar, updateDatetime);
        }

        public async Task UpdateGroupChatMessage(UpdateMessageRequest param, DateTime updateDatetime)
        {
            var currentMessage = await _groupChatMessageRepository.GetByIDAsync(param.MessageId);

            currentMessage.Content = param.Content;

            currentMessage.UpdatedAt = updateDatetime;

            await _groupChatMessageRepository.SaveChangeAsync();
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
