using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Helpers.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Services
{
    //public class ReactionCommentService /*: ReactionPostService, IReactionCommentService*/
    //{
    //    //private readonly IReactionBaseRepository<ReactionCommentEntity, ReactionCommentEntity> _reactionComment;
        //private readonly IMapper _mapper;
        //private readonly IHubContext<PostHub> _hubContext;
        //private readonly IEmotionTypeRepository _emotionTypeRepository;
        //private readonly IUserRepository _userRepository; // Bổ sung
        //private readonly IPostRepository _postRepository; // Bổ sung

        //public ReactionCommentService(
        //    IReactionBaseRepository<ReactionCommentEntity, ReactionCommentEntity> reactionComment,
        //    IMapper mapper,
        //    IEmotionTypeRepository emotionTypeRepository,
        //    IHubContext<PostHub> hubContext,
        //    IReactionBaseRepository<ReactionPostEntity, ReactionPostEntity> reactionPostRepository,
        //    IUserRepository userRepository, // Bổ sung dependency
        //    IPostRepository postRepository // Bổ sung dependency
        //)
        //: base(mapper, reactionPostRepository, emotionTypeRepository) // Gọi constructor của lớp cha đầy đủ các tham số
        //{
        //    _reactionComment = reactionComment;
        //    _mapper = mapper;
        //    _hubContext = hubContext;
        //    _emotionTypeRepository = emotionTypeRepository;
        //    _userRepository = userRepository;
        //    _postRepository = postRepository;
        //}

        //public async Task<IEnumerable<ReactionCommentEntity>> GetAllReactionsByCommentIdAsync(string postId)
        //{
        //    return await _reactionComment.GetAllReactionsIdAsync(postId);
        //}
    //}
}
