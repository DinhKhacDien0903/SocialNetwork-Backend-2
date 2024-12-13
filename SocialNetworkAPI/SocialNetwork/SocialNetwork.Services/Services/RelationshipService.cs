

namespace SocialNetwork.Services.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMapper _mapper;
        public RelationshipService(IRelationshipRepository relationshipRepository, IMapper mapper)
        {
            _relationshipRepository = relationshipRepository;
            _mapper = mapper;
        }

        public Task AccepFriendRequestAsync(string userId, string friendId)
        {
            var friend = _relationshipRepository.AccepFriendRequestAsync(userId,friendId);
            return friend;
        }

        public  Task CancelFriendAsync(string userId, string friendId)
        {
            var friend =  _relationshipRepository.CancelFriendAsync(userId, friendId);
            return friend;
        }

        public Task DeclineFriendAsync(string userId, string friendId)
        {
            var model=_relationshipRepository.DeclineFriendAsync(userId, friendId);
            return model;
        }

        public  Task DeclineFriendRequestAsync(string userId, string friendId)
        {
            var friend =  _relationshipRepository.DeclineFriendRequestAsync(userId, friendId);
            return friend;
        }

        public async Task<IEnumerable<UserSearchViewModel>> GetAllFriendAsync(string userId)
        {

            var getFriend=await _relationshipRepository.GetAllFriendAsync(userId);
            var friend=  _mapper.Map<IEnumerable<UserSearchViewModel>>(getFriend);
            return friend;
        }

        public async Task<IEnumerable<string>> GetFriendIdByUserId(string userId)
        {
            return await _relationshipRepository.GetFriendIdByUserId(userId);
        }

        public async Task<IEnumerable<UserSearchViewModel>> GetPendingFriendRequestAsync(string userId)
        {
            var getFriendPeding = await _relationshipRepository.GetPendingFriendRequestAsync(userId);
            var friend = _mapper.Map<IEnumerable<UserSearchViewModel>>(getFriendPeding);
            return friend;
        }

        public async Task<IEnumerable<UserSearchViewModel>> GetSendFriendRequestAsync(string userId)
        {
            var model = await _relationshipRepository.GetSendFriendRequestAsync(userId);
            var friend= _mapper.Map<IEnumerable<UserSearchViewModel>>(model);
            return friend;
        }

        public  Task SendFriendRequest(string friendId, string userId)
        {
            var getFriend =  _relationshipRepository.SendFriendRequestAsync(friendId,userId);
            return getFriend;
        }
    }
}
