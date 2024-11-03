namespace SocialNetwork.Services.IServices
{
    public interface IReactionHubService
    {
        Task<string> AddOrUpdateReaction(ReactionMessageRequest param);

        Task RemoveReactionByReactionIdAync(string reactionId);
    }
}
