namespace SocialNetwork.Domain.IRepositories
{
    public interface IRelationshipRepository : IBaseRepository<RelationshipEntity>
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
    }
}
