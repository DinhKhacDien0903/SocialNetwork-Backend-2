namespace SocialNetwork.Services.IServices
{
    public interface IRelationshipService
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
        Task SendFriendRequest(string friendId,string userId);
        Task AccepFriendRequestAsync(string friendId);
        Task DeclineFriendRequestAsync(string friendId);
        Task CancelFriendRequestAsync(string friendId);
        Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId);
        Task<IEnumerable<RelationshipEntity>> GetPendingFriendRequestAsync(string userId);
    }
}
