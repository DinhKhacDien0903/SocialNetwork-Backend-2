    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Domain.Entities;
    using SocialNetwork.DTOs.ViewModels;
    using System.Security.Claims;

    namespace SocialNetwork.Web.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PostController : ControllerBase
        {
            private readonly IPostService _postService;
            private readonly IPostHubService _postHubService;

            public PostController(IPostService postService, IPostHubService postHubService)
            {
                _postService = postService;
                _postHubService = postHubService;
            }
            [HttpGet("All")]
            public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAllPosts()
            {
                //var lastName=UserInforEntity.
                var posts = await _postService.GetAllPostsAsync();
                return Ok(posts);
            }


            [HttpGet("{id}")]
            public async Task<ActionResult<PostViewModel>> GetPostById(Guid id)
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound("Bài viết không tồn tại.");
                }
                return Ok(post);
            }

            [HttpPost]
            public async Task<ActionResult<PostRequest>> CreatePost(PostRequest postViewModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var createdPost = await _postService.CreatePostAsync(postViewModel, userId);

                await _postHubService.SendPostAsycn(createdPost);

                return Ok(createdPost);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult<PostViewModel>> UpdatePost(Guid id, [FromBody] PostViewModel postViewModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                postViewModel.PostID = id;
                var updatedPost = await _postService.UpdatePostAsync(postViewModel);

                if (updatedPost == null)
                {
                    return NotFound("Bài viết không tồn tại.");
                }

                //await _postHubService.SendUpdateAsycn(updatedPost);

                return Ok(updatedPost);
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> DeletePost(Guid id)
            {
                var result = await _postService.DeletePostAsync(id);
                if (!result)
                {
                    return NotFound("Bài viết không tồn tại.");
                }

                //await _postHubService.SendDeleteAsycn(id);

                return NoContent();
            }
        }
    }