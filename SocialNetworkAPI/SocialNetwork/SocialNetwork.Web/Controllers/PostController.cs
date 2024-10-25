using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Lấy tất cả bài viết thành công.",
                Data = posts
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostViewModel>> GetPostById(Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound(new BaseResponse
                {
                    Status = 404,
                    Message = "Bài viết không tồn tại."
                });
            }
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Lấy bài viết thành công.",
                Data = post
            });
        }

        [HttpPost]
        public async Task<ActionResult<PostViewModel>> CreatePost([FromBody] PostRequest postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    Status = 400,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState
                });
            }

            var createdPost = await _postService.CreatePostAsync(postViewModel);
            await _postHubService.SendPostAsycn(createdPost);

            // Sử dụng PostID từ createdPost khi tạo URL
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.PostID }, new BaseResponse
            {
                Status = 201,
                Message = "Bài viết đã được tạo thành công.",
                Data = createdPost
            });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<PostViewModel>> UpdatePost(Guid id, [FromBody] PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    Status = 400,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState
                });
            }

            postViewModel.PostID = id;
            var updatedPost = await _postService.UpdatePostAsync(postViewModel);
            if (updatedPost == null)
            {
                return NotFound(new BaseResponse
                {
                    Status = 404,
                    Message = "Bài viết không tồn tại."
                });
            }

            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Bài viết đã được cập nhật thành công.",
                Data = updatedPost
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result)
            {
                return NotFound(new BaseResponse
                {
                    Status = 404,
                    Message = "Bài viết không tồn tại."
                });
            }

            return NoContent();
        }
    }
}
