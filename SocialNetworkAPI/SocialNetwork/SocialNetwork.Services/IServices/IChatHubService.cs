namespace SocialNetwork.Services.IServices
{
    public interface IChatHubService
    {
        Task UpdateStatusActiveUser(string userId, bool isActive);

        Task<string> AddMessagePersonAsync(MessageViewModel messageViewModel);

        Task AddMessageImagesAsync(List<MessageImageViewModel> messageImages);
    }
}
