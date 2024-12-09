

namespace SocialNetwork.Services.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        public RelationshipService(IRelationshipRepository relationshipRepository)
        {
            _relationshipRepository = relationshipRepository;
        }

        public Task AccepFriendRequestAsync(string friendId)
        {
            var friend = _relationshipRepository.AccepFriendRequestAsync(friendId);
            return friend;
        }

        public  Task CancelFriendRequestAsync(string friendId)
        {
            var friend =  _relationshipRepository.CancelFriendRequestAsync(friendId);
            return friend;
        }

        public  Task DeclineFriendRequestAsync(string friendId)
        {
            var friend =  _relationshipRepository.DeclineFriendRequestAsync(friendId);
            return friend;
        }

        public async Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId)
        {
            var getFriend=await _relationshipRepository.GetAllFriendAsync(userId);
            return getFriend;
        }

        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _relationshipRepository.GetFriendIdByUserId(userId);
        }

        public async Task<IEnumerable<RelationshipEntity>> GetPendingFriendRequestAsync(string userId)
        {
            var getFriendPeding = await _relationshipRepository.GetPendingFriendRequestAsync(userId);
            return getFriendPeding;
        }

        public  Task SendFriendRequest(string friendId, string userId)
        {
            var getFriend =  _relationshipRepository.SendFriendRequestAsync(friendId,userId);
            return getFriend;
        }
    }
}
