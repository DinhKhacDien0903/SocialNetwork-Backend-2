using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IReactionPostService _reactionPostService;
        private readonly IPostHubService _postHubService;

        public PostController(IPostService postService,
             IReactionPostService reactionPostService,
            IPostHubService postHubService)
        {
            _postService = postService;
            _reactionPostService = reactionPostService;
            _postHubService = postHubService;
        }


  
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAllPosts(int pageIndex=1, int pageSize=10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var posts = await _postService.GetAllPostsAsync(userId,pageIndex,pageSize);
            //return Ok(posts);
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "get all post success",
                Data = posts
            });
        }

        [HttpGet("not-approved")]
        public async Task<ActionResult<IEnumerable<AdminBrowsePostViewModel>>> GetAllPostsAdmin()
        {   
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var posts = await _postService.GetAdminBrowseAsync();
            return Ok(posts);
        }

        [HttpPost("deletePost/{postId}")]
        public async Task<ActionResult> DeletePost(string postId)
        {
            var result=await _postService.DeletePostAsync(postId);
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "delete post is success"
            });
        }

        [HttpGet("User")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPostsByUserIdAsync([FromQuery]string userId, int pageIndex, int pageSixe)
        {
            var posts = await _postService.GetPostsByUserIdAsync(userId,pageIndex,pageSixe);
            return Ok(new BaseResponse
            {
                Status=200,
                Message="get all post of user is success",
                Data = posts
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostViewModel>> GetPostById(string id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound("Bài viết không tồn tại.");
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> CreatePost( PostRequest postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdPost = await _postService.CreatePostAsync(postViewModel, userId);
            //await _postHubService.SendPostAsync(createdPost);
            return CreatedAtAction(nameof(CreatePost), new { postId = createdPost.PostID }, createdPost);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<PostViewModel>> UpdatePost(string id, [FromBody] PostViewModel postViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    postViewModel.PostID = id;
        //    var updatedPost = await _postService.UpdatePostAsync(postViewModel);

        //    if (updatedPost == null)
        //    {
        //        return NotFound("Bài viết không tồn tại.");
        //    }

        //    //await _postHubService.SendUpdateAsycn(updatedPost);
        //    return Ok(updatedPost);
        //}

       

        /// <summary>
        /// Emotion
        /// </summary>
        /// <returns></returns>

        [HttpGet("AllEmotion")]
        public async Task<ActionResult<IEnumerable<EmotionRequest>>> GetAllEmotion()
        {
            var emotions = await _reactionPostService.GetAllEmotionTypesAsync();
            return Ok(emotions);
        }

        [HttpPut("emotion/{postId}")]
        public async Task<IActionResult> AddEmotion(string postId, EmotionRequest emotionRequest)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _reactionPostService.AddReactionAsync(postId, userId, emotionRequest.EmotionTypeID);

            if (result == null)
            {
                return BadRequest("error add reaction");
            }
            return Ok(result);
        }

        [HttpDelete("emotion/{postId}")]

        public async Task<IActionResult> CancelReleaseEmotion(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _reactionPostService.RemoveReactionAsync(postId, userId);
            if (!result) return BadRequest("Failed to delete reaction");
            return Ok(new { PostID = postId, UserID = userId });
        }
    }
}