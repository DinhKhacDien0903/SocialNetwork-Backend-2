namespace SocialNetwork.Services.IServices
{
    public interface IChatHubService
    {
        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<MessageViewModel> AddMessagePersonAsync(MessageViewModel messageViewModel);

        Task<NotificationViewModel> AddNotificationToUserAsync(NotificationViewModel notifiationViewModel);

        Task<IEnumerable<NotificationViewModel>> GetAllNotificationMessageAsync(string userId);

        Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages);

        Task RemoveMessage(string messageId);

        Task UpdateMessage(UpdateMessageRequest param, DateTime updateDatetime);
    }
}
