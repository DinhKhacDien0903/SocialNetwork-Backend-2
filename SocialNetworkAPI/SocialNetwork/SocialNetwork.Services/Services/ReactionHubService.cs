
namespace SocialNetwork.Services.Services
{
    public class ReactionHubService : IReactionHubService
    {
        private readonly IReactionRepository _reactionRepository;

        private readonly IReactionMessageRepository _reactionMessageRepository;

        private readonly IReactionGroupChatMessageRepository _reactionGroupChatMessageRepository;

        private readonly IMapper _mapper;

        public ReactionHubService(
            IReactionRepository reactionRepository,
            IReactionMessageRepository reactionMessageRepository,
            IReactionGroupChatMessageRepository reactionGroupChatMessageRepository,
            IMapper mapper)
        {
            _reactionRepository = reactionRepository;
            _reactionMessageRepository = reactionMessageRepository;
            _reactionGroupChatMessageRepository = reactionGroupChatMessageRepository;
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

        public async Task<string> AddOrUpdateReactionGroupChatMessage(ReactionMessageRequest param)
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

                return await AddReactionAsync(param, true);
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

        private async Task<string> AddReactionAsync(ReactionMessageRequest param, bool isGroupChatMessage = false)
        {
            var reactionID = Guid.NewGuid().ToString();

            var entity = new ReactionEntity
            {
                ReactionID = reactionID,
                UserID = param.SenderId,
                EmotionTypeID = param.EmotionType
            };

            await _reactionRepository.AddAsync(entity);
            await _reactionRepository.SaveChangeAsync();

            if (isGroupChatMessage)
            {
                var reactionMessageEntity = new ReactionGroupChatMessageEntity
                {
                    ReactionID = reactionID,
                    GroupChatMessageID = param.MessageId
                };

                await _reactionGroupChatMessageRepository.AddAsync(reactionMessageEntity);
                await _reactionGroupChatMessageRepository.SaveChangeAsync();      
            }
            else
            {

                var reactionMessageEntity = new ReactionMessageEntity
                {
                    ReactionID = reactionID,
                    MessageID = param.MessageId
                };

                await _reactionMessageRepository.AddAsync(reactionMessageEntity);
                await _reactionMessageRepository.SaveChangeAsync();
            }

            return reactionID;
        }
    }
}