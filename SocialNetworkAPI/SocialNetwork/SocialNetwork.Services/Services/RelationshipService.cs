
namespace SocialNetwork.Services.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        public RelationshipService(IRelationshipRepository relationshipRepository)
        {
            _relationshipRepository = relationshipRepository;
        }
        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _relationshipRepository.GetFriendIdByUserId(userId);
        }
    }
}
