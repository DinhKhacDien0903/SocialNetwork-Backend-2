using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Helpers.Hubs;

namespace SocialNetwork.Services.Services
{
    public class ReactionPostService : IReactionPostService
    {
        private readonly IReactionPostRepository _reactionPost;
        private readonly IMapper _mapper;
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IEmotionTypeRepository _emotionTypeRepository;
        public ReactionPostService(IMapper mapper,
            IReactionPostRepository reactionPost,
            IEmotionTypeRepository emotionTypeRepository,
            IHubContext<PostHub> hubContext
            )
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _emotionTypeRepository = emotionTypeRepository;
            _reactionPost = reactionPost;
        }

        public async Task<bool> AddReactionAsync(string postId, string userId, string emotionTypeId)
        {
            var emotion = await _reactionPost.GetByPostIdAndUserIdAsync(postId, userId);
            if (emotion != null)
            {
                emotion.Reaction.EmotionTypeID = emotionTypeId;
                await _reactionPost.UpdateAsync(emotion);
                return true;
            }
            var reaction = new ReactionEntity
            {
                UserID = userId,
                EmotionTypeID = emotionTypeId,
                IsDeleted = false
            };
            var newEmotion = new ReactionPostEntity
            {
                ReactionID = Guid.NewGuid().ToString(),
                PostID = postId,
                Reaction=reaction,
                //Reaction = new ReactionEntity
                //{
                //    UserID = userId,
                //    EmotionTypeID = emotionTypeId,
                //    IsDeleted = false
                //}
            };
            await _reactionPost.AddAsync(newEmotion);
            await _hubContext.Clients.All.SendAsync("ReceiveReaction", postId, userId, emotionTypeId);

            return true;
        }

        public async Task<IEnumerable<EmotionTypeEntity>> GetAllEmotionTypesAsync()
        {
            return await _emotionTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ReactionPostEntity>> GetAllReactionsByPostIdAsync(string postId)
        {
            return await _reactionPost.GetAllReactionsByPostIdAsync(postId);

        }

        public async Task<bool> RemoveReactionAsync(string postId, string userId)
        {
            var emotion=await _reactionPost.GetByPostIdAndUserIdAsync(postId,userId);
            if (emotion != null)
            {
                await _reactionPost.DeleteAsync(emotion);
                return true;
            }
            return false;
        }
    }
}
