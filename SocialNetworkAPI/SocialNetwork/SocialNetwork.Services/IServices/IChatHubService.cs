namespace SocialNetwork.Services.IServices
{
    public interface IChatHubService
    {
        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<MessageViewModel> AddMessagePersonAsync(MessageViewModel messageViewModel);
        Task<GroupChatMessageViewModel> AddMessageGroupAsync(GroupChatMessageViewModel messageViewModel);

        Task<NotificationViewModel> AddNotificationToUserAsync(NotificationViewModel notifiationViewModel);

        Task<IEnumerable<NotificationViewModel>> GetAllNotificationMessageAsync(string userId);

        Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages);
        Task AddMessageImagesGroupChatAsync(List<GroupChatMessageImageViewModel> messageImages);

        Task RemoveMessage(string messageId);

        Task RemoveGroupChatMessage(string messageId);
        Task UpdateMessage(UpdateMessageRequest param, DateTime updateDatetime);

        Task UpdateGroupChatMessage(UpdateMessageRequest param, DateTime updateDatetime);

        Task UpdateGroupChatAvatar(UpdateGroupChatRequest param, DateTime updateDatetime);

        Task<NotificationViewModel> ReadMessageNotificationAsync(string userId, string friendId, string groupId);

        Task<bool> IsNotificationExist(string senderId, string reciverId);
    }
}
