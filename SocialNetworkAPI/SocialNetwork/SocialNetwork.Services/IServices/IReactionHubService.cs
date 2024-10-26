namespace SocialNetwork.Services.IServices
{
    public interface IReactionHubService
    {
        Task AddReaction(ReactionMessageRequest param, string userId);
    }
}
