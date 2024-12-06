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
        private readonly INotificationService _notificationService;

        public UserController(IUserService userServices, INotificationService notificationService = null)
        {
            _userServices = userServices;
            _notificationService = notificationService;
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

        [Authorize]
        [HttpGet("SearchUser")]
        public async Task<IActionResult> GetSearchUserAsync(string userSearch)
        {
            try
            {
                var user = await _userServices.SearchUserByNameAsync(userSearch);
                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get search user success",
                    Data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotification()
       {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var notification = await _notificationService.GetUserNotificationAsync(userId);
                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get Notification is success",
                    Data = notification
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _notificationService.MarkNotificationAsync(id);
            return Ok(new { Success = true });
        }

    }
}
