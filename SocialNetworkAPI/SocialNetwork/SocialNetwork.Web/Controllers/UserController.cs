using Microsoft.AspNetCore.Authorization;
using SocialNetwork.Domain;
using SocialNetwork.DTOs.Authorize;
using SocialNetwork.DTOs.ViewModels;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly INotificationService _notificationService;
        private readonly IRelationshipService _relationshipService;
        private readonly IPostHubService _postHubService;



        public UserController(IUserService userServices, INotificationService notificationService = null, IRelationshipService relationshipService = null, IPostHubService postHubService = null)
        {
            _userServices = userServices;
            _notificationService = notificationService;
            _relationshipService = relationshipService;
            _postHubService = postHubService;
        }

        #region

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
        public async Task<IActionResult> GetUserInfor(string? userId)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId))
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        #endregion



        #region
        [Authorize]
        [HttpGet("SearchUser")]
        public async Task<IActionResult> GetSearchUserAsync([FromQuery] SearchQuery userSearch)
       {
            try
            {
                var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userServices.SearchUserByNameAsync(userSearch,userId);
                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Lấy thông tin người dùng tìm kiếm thành công",
                    Data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Authorize]
        [HttpGet("notificationFriend")]
        public async Task<IActionResult> GetNotificationFriend()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var notification = await _notificationService.GetAllFriendRequest(userId);
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
        #endregion



        #region
        [Authorize]
        [HttpPost("Send")]
        public async Task<IActionResult> SendFriendRequest([FromBody] string friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var friendInf= await _relationshipService.SendFriendRequest(userId, friendId); 
            return Ok(new BaseResponse
            {
                Status=200,
                Message="send friend request success",
                Data = friendInf
            });
        }

        [Authorize]
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] string friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _relationshipService.AccepFriendRequestAsync(userId, friendId);
            return Ok(new { Message = "accep friend request is success" });
        }

        //huy kb
        [Authorize]
        [HttpPost("cancel/{friendId}")]
        public async Task<IActionResult> CancelFriend(string friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _relationshipService.CancelFriendAsync(userId, friendId);
            await  _postHubService.CancelFriend(userId, friendId);
            return Ok(new { Message = "cancel friend request is success" });
        }



        // thu hoi
        [Authorize]
        [HttpPost("cancelRequest/{friendId}")]
        public async Task<IActionResult> DeclineFriendRequest(string friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _relationshipService.DeclineFriendRequestAsync(userId, friendId);
            return Ok(new { Message = "decline friend request is success" });
        }



        //xoa request 
        [Authorize]
        [HttpPost("decline/{friendId}")]
        public async Task<IActionResult> DeclineFriend(string friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _relationshipService.DeclineFriendAsync(userId, friendId);
            return Ok(new { Message = "decline friend request is success" });
        }

        [Authorize]
        [HttpGet("friends")]
        public async Task<IActionResult> GetAllFriend()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var friend=await _relationshipService.GetAllFriendAsync(userId);
            return Ok(new BaseResponse
            {
                Status = 200,
                Message="get all friend is success",
                Data = friend
            });  
        }


        [Authorize]
        [HttpGet("request")]
        public async Task<IActionResult> GetAllPedingFriend()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var friend = await _relationshipService.GetPendingFriendRequestAsync(userId);
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "get all peding friend is success",
                Data = friend
            });
        }

        [Authorize]
        [HttpGet("sendRequest")]
        public async Task<IActionResult> GetAllSendRequestFriend()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sendRequest= await _relationshipService.GetSendFriendRequestAsync(userId);
            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "get all send request is success",
                Data = sendRequest
            });
        }
        #endregion

        [Authorize(Roles = ApplicationRoleModel.User)]
        [HttpPut("updateInfor")]
        public async Task<IActionResult> UpdateUserInfor(UserViewModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("You must login to update your informations");
            }

            request.Id = userId;

            var user = await _userServices.UpdateUserInforAsync(request);

            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Get user infor success",
                Data = user
            });
        }
    }
}
