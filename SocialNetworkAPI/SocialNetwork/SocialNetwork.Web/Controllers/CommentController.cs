using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            var comment = await _commentService.GetAllCommentAsync();
            return Ok(comment);
        }
        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetCommentByPostId(string postId)
        {
            var commnet = await _commentService.GetCommentByIdAsync(postId);
            return Ok(commnet);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(string commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpGet("replies/{parentCommentId}")]
        public async Task<IActionResult> GetRepliesByCommentId(string parentCommentId)
        {
            var replies = await _commentService.GetRepliesByCommentIdAsync(parentCommentId);
            return Ok(replies);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addComment = await _commentService.AddCommentAsync(commentRequest);
            //return CreatedAtAction(nameof(GetCommentById), new { commentId = addComment.CommentID }, addComment);
            return Ok(addComment);
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

    }

}

