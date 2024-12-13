namespace SocialNetwork.Domain.IRepositories
{
    public interface IRelationshipRepository : IBaseRepository<RelationshipEntity>
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
        Task SendFriendRequestAsync(string userId,string friendId);
        Task AccepFriendRequestAsync(string userId, string friendId);
        Task DeclineFriendRequestAsync(string userId, string friendId);
        Task CancelFriendAsync(string userId, string friendId);
        Task DeclineFriendAsync(string userId, string friendId);
        Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId);
        Task<IEnumerable<UserEntity>> GetPendingFriendRequestAsync(string userId);
        Task<IEnumerable<UserEntity>> GetSendFriendRequestAsync(string userId);
    }
}
