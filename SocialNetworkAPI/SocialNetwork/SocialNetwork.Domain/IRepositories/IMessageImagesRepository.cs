namespace SocialNetwork.Domain.IRepositories
{
    public interface IMessageImagesRepository : IBaseRepository<MessageImageEntity>
    {
        Task<List<string>> GetAllImageByMessageId(string MessageId);
    }
}
