namespace SocialNetwork.Services.IServices
{
    public interface IRelationshipService
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
    }
}
