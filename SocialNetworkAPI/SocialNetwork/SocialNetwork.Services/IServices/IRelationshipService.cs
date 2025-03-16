namespace SocialNetwork.Services.IServices
{
    public interface IRelationshipService
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
        Task<FriendRequestViewmodel> SendFriendRequest(string friendId,string userId);
        Task AccepFriendRequestAsync(string userId, string friendId);
        Task DeclineFriendRequestAsync(string userId, string friendId);
        Task DeclineFriendAsync(string userId, string friendId);
        Task CancelFriendAsync(string userId, string friendId);
        Task<IEnumerable<UserSearchViewModel>> GetAllFriendAsync(string userId);
        Task<IEnumerable<UserSearchViewModel>> GetPendingFriendRequestAsync(string userId);
        Task<IEnumerable<UserSearchViewModel>> GetSendFriendRequestAsync(string userId);

    }
}
