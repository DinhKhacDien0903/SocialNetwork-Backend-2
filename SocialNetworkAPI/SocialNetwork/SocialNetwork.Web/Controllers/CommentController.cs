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

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetCommentByPostId(Guid postId)
        {
            var comments=await _commentService.GetCommentByPostIdAsycn(postId);
            if (comments == null || !comments.Any())
            {
                return NotFound("Không có bình luận nào trong trong bài viết");
            }
            return Ok(comments);
        }

        [HttpGet("commentId")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetRepliesByCommentId(Guid commentId)
        {
            var replies=await _commentService.GetRepliesByCommentIdAsycn(commentId);
            if (replies == null || !replies.Any())
            {
                return NotFound("Không có phản hồi nào trong trong bình luận này");
            }
            return Ok(replies);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment( CommentViewModel commentViewModel)
        {
            

            var createdComment = await _commentService.AddCommentAsycn(commentViewModel);
            return CreatedAtAction(nameof(GetCommentByPostId), new { commentId = createdComment.CommentID }, createdComment);
        }

        [HttpPut("commentId")]  
        public async Task<ActionResult> UpdateComment(Guid commentId,  CommentViewModel commentView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            await _commentService.UpdateCommentAsycm(commentId, commentView);
            return Ok("Update comment success" + commentView);
        }

        [HttpDelete("commentId")]
        public async Task<ActionResult> DeleteComment(Guid commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _commentService.DeleteCommentAsycn(commentId);

            return Ok("delete comment success");
        }

    }
}
