namespace SocialNetwork.Domain.IRepositories
{
    public interface IRelationshipRepository : IBaseRepository<RelationshipEntity>
    {
        Task<IEnumerable<string>> GetFriendIdByUserId(string userId);
        Task SendFriendRequestAsync(string userId,string friendId);
        Task AccepFriendRequestAsync(string  friendId);
        Task DeclineFriendRequestAsync(string friendId);
        Task CancelFriendRequestAsync(string friendId); 
        Task<IEnumerable<UserEntity>> GetAllFriendAsync(string userId);
        Task<IEnumerable<RelationshipEntity>> GetPendingFriendRequestAsync(string userId);
    }
}
