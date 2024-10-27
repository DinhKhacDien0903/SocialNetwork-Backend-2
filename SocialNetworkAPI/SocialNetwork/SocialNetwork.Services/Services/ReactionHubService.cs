
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
        public async Task<string> AddReaction(ReactionMessageRequest param, string userId)
        {
            try
            {
                var reactionID = Guid.NewGuid().ToString();

                var entity = new ReactionEntity
                {
                    ReactionID = reactionID,
                    UserID = userId,
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
            catch(Exception e)
            {
                throw new Exception("Error when add reaction to database " + e.Message);
            }
        }
    }
}
