using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Services.Services
{
    public class ReactionPostService : IReactionPostService
    {
        private readonly IReactionPostRepository _reactionPost;
        private readonly IReactionRepository _reaction;
        private readonly IMapper _mapper;

        public ReactionPostService(IMapper mapper, IReactionPostRepository reactionPost, IReactionRepository reaction)
        {
            _mapper = mapper;
            _reactionPost = reactionPost;
            _reaction = reaction;
        }

        public async Task AddReactionAsycn(ReactionRequest model)
        {
            var hasUser = await _reactionPost.UserHasReactionAsync(model.UserID, model.PostID);
            if (hasUser)
            {
                throw new Exception("User has already reacted to the post.");
            }

            var reaction = _mapper.Map<ReactionEntity>(model);
            await _reaction.AddAsync(reaction);

            var reactionPost = _mapper.Map<ReactionPostEntity>(model);
            reactionPost.ReactionID = reaction.ReactionID;

            await _reactionPost.AddReactionAsync(reactionPost);
        }

        public async Task<IEnumerable<ReactionPostViewModel>> GetReactionByPostIdAsync(Guid postId)
        {
            var reactions = await _reactionPost.GetReactionsByPostIdAsync(postId);
            if (reactions == null)
            {
                throw new Exception($"No reactions found for postId: {postId}");
            }

            return _mapper.Map<IEnumerable<ReactionPostViewModel>>(reactions);
        }

        public async Task DeleteReactionAsycn(Guid reactionId, Guid postId)
        {
            var reactionPost = await _reactionPost.GetReactionsByPostIdAsync(reactionId);
            if (reactionPost == null)
            {
                throw new Exception($"Reaction with id {reactionId} for post {reactionId} not found.");
            }

            await _reactionPost.DeleteReactionAsync(reactionId,postId);
        }

        public async Task UpdateReactionAsync(ReactionPostViewModel model)
        {
            var existingReactionPost = await _reactionPost.GetReactionsByPostIdAsync( model.PostID);

            if (existingReactionPost == null)
            {
                throw new Exception($"Reaction with id {model.ReactionID} for post {model.PostID} not found.");
            }

            var reactionPost=existingReactionPost.FirstOrDefault(s=>s.ReactionID==model.ReactionID);


            var reactionEntity = reactionPost.Reaction;

            if (reactionEntity != null)
            {
                _mapper.Map(reactionEntity,model);
                await _reaction.UpdateAsync(reactionEntity);
            }

            var reactionPostAuto = _mapper.Map<ReactionPostEntity>(model);
            await _reactionPost.UpdateReactionAsync(reactionPostAuto);
        }

    }
}
