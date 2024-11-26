namespace SocialNetwork.Services.IServices
{
    public interface IChatHubService
    {
        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<MessageViewModel> AddMessagePersonAsync(MessageViewModel messageViewModel);

        Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages);

        Task RemoveMessage(string messageId);

        Task UpdateMessage(UpdateMessageRequest param, DateTime updateDatetime);
    }
}
