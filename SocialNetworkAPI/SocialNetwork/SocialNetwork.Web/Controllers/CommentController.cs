using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.DTOs.ViewModels;
using SocialNetwork.Helpers.Hubs;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IHubContext<PostHub> _postHubContext;
        private readonly ICommentService _commentService;
        //private readonly IReactionCommentService _reactionComment;

        public CommentController(ICommentService commentService, IHubContext<PostHub> postHubContext)
        {
            _commentService = commentService;
            _postHubContext = postHubContext;
            //_reactionComment = reactionComment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            var comment = await _commentService.GetAllCommentAsync();
            return Ok(comment);
        }



        [HttpGet("{postId}")]
        public async Task<IActionResult> GetCommentByPostId(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var commnet = await _commentService.GetCommentByPostIdAsync(postId);
            return Ok(new
            {
                numberOfComment = commnet.NumberOfComment,
                comment = commnet.Comment
            });
        }


        //[HttpGet("replies/{parentCommentId}")]
        //public async Task<IActionResult> GetRepliesByCommentId(string parentCommentId)
        //{
        //    var replies = await _commentService.GetRepliesByCommentIdAsync(parentCommentId);
        //    return Ok(replies);
        //}

        [HttpPost]
        public async Task<ActionResult<CommentViewModel>> AddComment(CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var addComment = await _commentService.AddCommentAsync(commentRequest,userId);
            //await _postHubContext.Clients.Group(addComment.PostID.ToString())
            //.SendAsync("ReceiveComment", addComment);
            return CreatedAtAction(nameof(AddComment), new { commentId = addComment.CommentID }, addComment);
            //return Ok(addComment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(CommentViewModel commentViewModel, string commentId)
        {
            if (commentId != commentViewModel.CommentID)
            {
                return BadRequest("Comment id not found");
            }

            await _commentService.UpdateCommentAsync(commentViewModel);
            return NoContent();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComent(string id)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();

        }

        [HttpGet("count/{postId}")]
        public async Task<IActionResult> GetCommentCount(string postId)
        {
            var count = await _commentService.GetCommentCountByPostIdAsync(postId);
            return Ok(new { count });
        }

        //[HttpGet("AllEmotion")]
        //public async Task<ActionResult<IEnumerable<EmotionRequest>>> GetAllEmotion()
        //{
        //    var emotions = await _reactionComment.GetAllEmotionTypesAsync();
        //    return Ok(emotions);
        //}

        //[HttpPut("emotion/{postId}")]
        //public async Task<IActionResult> AddEmotion(string postId, EmotionRequest emotionRequest)
        //{

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var result = await _reactionComment.AddReactionAsync(postId, userId, emotionRequest.EmotionTypeID);

        //    if (result == null)
        //    {
        //        return BadRequest("error add reaction");
        //    }
        //    return Ok();
        //}

        //[HttpDelete("emotion/{postId}")]

        //public async Task<IActionResult> CancelReleaseEmotion(string postId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var result = await _reactionComment.RemoveReactionAsync(postId, userId);
        //    return result ? Ok() : NotFound("no have reaction remove");
        //}

    }

}

