
namespace SocialNetwork.Services.Services
{
    public class ReactionHubService : IReactionHubService
    {
        private readonly IReactionRepository _reactionRepository;

        private readonly IMapper _mapper;

        public ReactionHubService(
            IReactionRepository reactionRepository,
            IMapper mapper)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }
        public async Task AddReaction(ReactionMessageRequest param, string userId)
        {
            var entity = new ReactionEntity
            {
                ReactionID = Guid.NewGuid().ToString(),
                UserID = userId,
                EmotionTypeID = param.EmotionType
            };

            await _reactionRepository.AddAsync(entity);
        }
    }
}
