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
        private readonly IReactionBaseRepository<ReactionPostEntity, ReactionPostEntity> _reactionPost;
        private readonly IMapper _mapper;
        private readonly IPostHubService _postHubService;
        private readonly IUserRepository _userRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        private readonly IEmotionTypeRepository _emotionTypeRepository;
        public ReactionPostService(IMapper mapper,
            IReactionBaseRepository<ReactionPostEntity, ReactionPostEntity> reactionPost,
            IEmotionTypeRepository emotionTypeRepository,
            IUserRepository userRepository,
            IPostRepository postRepository,
            IPostHubService postHubService,
            IReactionRepository reactionRepository)
        {
            _mapper = mapper;
            _emotionTypeRepository = emotionTypeRepository;
            _reactionPost = reactionPost;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postHubService = postHubService;
            _reactionRepository = reactionRepository;
        }

        public async Task<ReactionRequest> AddReactionAsync(string postId, string userId, string emotionTypeId)
        {
            var existingReaction = await _reactionPost.GetIdAndUserIdAsync(postId, userId);
            var user = await _userRepository.GetByIDAsync(userId);
            //var post =await _postRepository.GetByIDAsync(postId);
            var emoName = await _reactionRepository.GetByIDAsync(emotionTypeId);
            var reactionRequest = new ReactionRequest
            {
                PostID = postId,
                UserID = userId,
                EmotionTypeID = emotionTypeId,
                EmotionName = emoName.EmotionName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarUrl = user.AvatarUrl,
                //CreatedAt = existingReaction.Reaction.CreatedAt,
            };
            if (existingReaction != null)
            {
                var reaction = await _reactionPost.GetReactionByReactionIdAsync(existingReaction.ReactionID);
                existingReaction.Reaction.EmotionTypeID = emotionTypeId;
                await _reactionPost.UpdateAsync(existingReaction);

                await _postHubService.SendReactionUpdateAsycn(reactionRequest);

                return reactionRequest;
            }
            //new
            var reactionNew = new ReactionEntity
            {
                ReactionID=Guid.NewGuid().ToString(),
                UserID = userId,
                User= await _userRepository.GetByIDAsync(userId),
                EmotionTypeID = emotionTypeId,
                IsDeleted = false
            };

            var newReactionPost = new ReactionPostEntity
            {
                //ReactionID = Guid.NewGuid().ToString(),
                Post=await _postRepository.GetByIDAsync(postId),
                PostID = postId,
                Reaction = reactionNew,
                ReactionID= reactionNew.ReactionID
                
            };

            await _reactionPost.AddAsync(newReactionPost);

           

            

            await _postHubService.SendReactionAddAsycn(reactionRequest);
            return reactionRequest;
        }


        public async Task<IEnumerable<EmotionTypeEntity>> GetAllEmotionTypesAsync()
        {
            return await _emotionTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ReactionPostEntity>> GetAllReactionsByPostIdAsync(string postId)
        {
            return await _reactionPost.GetAllReactionsIdAsync(postId);

        }

        public async Task<bool> RemoveReactionAsync(string postId, string userId)
        {
            var emotion=await _reactionPost.GetIdAndUserIdAsync(postId,userId);
            if (emotion != null)
            {
                await _reactionPost.DeleteAsync(emotion);

                await _postHubService.SendDeleteReactionAsycn(postId, userId);
                return true;
            }
            return false;
        }
    }
}
