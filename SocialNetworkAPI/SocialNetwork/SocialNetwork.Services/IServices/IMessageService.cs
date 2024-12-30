using SocialNetwork.DTOs.Response;

namespace SocialNetwork.Services.IServices
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagePersonResponse>> GetAllMessagesAsync(string userId, string receiverId);
    }
}
