
namespace SocialNetwork.Services.Services
{
    public class ReactionHubService : IReactionHubService
    {
        private readonly IReactionRepository _reactionRepository;

        private readonly IReactionMessageRepository _reactionMessageRepository;

        private readonly IMapper _mapper;

        public ReactionHubService(
            IReactionRepository reactionRepository,
            IReactionMessageRepository reactionMessageRepository,
            IMapper mapper)
        {
            _reactionRepository = reactionRepository;
            _reactionMessageRepository = reactionMessageRepository;
            _mapper = mapper;
        }
        public async Task<string> AddOrUpdateReaction(ReactionMessageRequest param)
        {
            try
            {
                var reaction = await _reactionRepository.GetReactionIdByMessageIdAndUserId(param);

                if (reaction != null)
                {
                    reaction.EmotionTypeID = param.EmotionType;

                    reaction.UpdatedAt = DateTime.UtcNow;

                    _reactionRepository.Update(reaction);

                    await _reactionRepository.SaveChangeAsync();

                    return reaction.ReactionID;
                }

                return await AddReactionAsync(param);
            }
            catch (Exception e)
            {
                throw new Exception("Error when add reaction to database " + e.Message);
            }
        }

        public async Task RemoveReactionByReactionIdAync(string reactionId)
        {
            try
            {

                var currentReaction = await _reactionRepository.GetByIDAsync(reactionId);

                _reactionRepository.Delete(currentReaction);

                await _reactionRepository.SaveChangeAsync();

            }
            catch (Exception e)
            {
                throw new Exception("Error when remove reaction to database" + e.Message);
            }
        }

        private async Task<string> AddReactionAsync(ReactionMessageRequest param)
        {
            var reactionID = Guid.NewGuid().ToString();

            var entity = new ReactionEntity
            {
                ReactionID = reactionID,
                UserID = param.SenderId,
                EmotionTypeID = param.EmotionType
            };

            var reactionMessageEntity = new ReactionMessageEntity
            {
                ReactionID = reactionID,
                MessageID = param.MessageId
            };

            await _reactionRepository.AddAsync(entity);

            await _reactionMessageRepository.AddAsync(reactionMessageEntity);

            await _reactionRepository.SaveChangeAsync();

            return reactionID;
        }
    }
}