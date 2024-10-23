using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DTOs.Authorize;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [Authorize(Roles = ApplicationRoleModel.User)]
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userServices.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound(new BaseResponse
                {
                    Status = 404,
                    Message = "Not Found User In Server"
                });
            }

            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Get all success success",
                Data = users
            });
        }

        [Authorize]
        [HttpGet("getInfor")]
        public async Task<IActionResult> GetUserInfor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId == null)
            {
                return Unauthorized("You must login to get your informations");
            }   

            var user = await _userServices.GetUserInforAsync(userId);

            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Get user infor success",
                Data = user
            });
        }

        [Authorize]
        [HttpGet("getFriendOnline")]
        public async Task<IActionResult> GetFriendOnlinesAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var friendOnlines = await _userServices.GetFriendOnlinesAsync(userId);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get friend onlines success",
                    Data = friendOnlines
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
