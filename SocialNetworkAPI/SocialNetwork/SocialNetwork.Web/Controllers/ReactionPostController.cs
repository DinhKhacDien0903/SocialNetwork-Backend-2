using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionPostController : ControllerBase
    {
        private readonly IReactionPostService _reactionPostService;

        public ReactionPostController(IReactionPostService reactionPostService)
        {
            _reactionPostService = reactionPostService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction(ReactionRequest model)
        {
            await _reactionPostService.AddReactionAsycn(model);
            return Ok(model);
        }


        [HttpPut("{post}")]
        public async Task<IActionResult> UpdateReaction(ReactionPostViewModel model)
        {
            await _reactionPostService.UpdateReactionAsync(model);
            return Ok(model);
        }


        [HttpGet("{postId}")]
        public async Task<IActionResult> GetReactionByPostId(Guid postId)
        {
            var reaction=await _reactionPostService.GetReactionByPostIdAsync(postId);
            return Ok(reaction);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteReation(Guid reactionId, Guid postId)
        {
            await _reactionPostService.DeleteReactionAsycn(reactionId,postId);
            return Ok();
        }


    }
}
