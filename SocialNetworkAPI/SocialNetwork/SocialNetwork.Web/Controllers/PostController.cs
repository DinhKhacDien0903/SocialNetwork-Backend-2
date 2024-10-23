using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // Lấy tất cả các bài viết
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        // Lấy bài viết theo ID
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

            var createdPost = await _postService.CreatePostAsync(postViewModel);
            return Ok(createdPost);
        }



        // Cập nhật bài viết
        [HttpPut("{id}")]
        public async Task<ActionResult<PostViewModel>> UpdatePost(Guid id, [FromBody] PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            postViewModel.PostID = id; // Đảm bảo ID trong URL và body khớp nhau
            var updatedPost = await _postService.UpdatePostAsync(postViewModel);
            if (updatedPost == null)
            {
                return NotFound("Bài viết không tồn tại.");
            }

            return Ok(updatedPost);
        }

        // Xóa bài viết (soft delete)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result)
            {
                return NotFound("Bài viết không tồn tại.");
            }

            return NoContent();
        }
    }
}
