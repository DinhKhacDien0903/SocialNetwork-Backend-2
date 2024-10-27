namespace SocialNetwork.Services.IServices
{
    public interface IReactionHubService
    {
        Task<string> AddReaction(ReactionMessageRequest param, string userId);
    }
}
