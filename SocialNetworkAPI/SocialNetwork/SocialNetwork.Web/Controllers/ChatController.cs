using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DTOs.ViewModels;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        private readonly IConversationService _conversationService;

        private readonly IGroupChatService _groupChatService;

        private readonly IChatHubService _chatHubService;
        public ChatController(
            IMessageService messageService,
            IConversationService conversationService,
            IGroupChatService groupChatService,
            IChatHubService chatHubService)
        {
            _messageService = messageService;
            _conversationService = conversationService;
            _groupChatService = groupChatService;
            _chatHubService = chatHubService;
        }

        [Authorize]
        [HttpGet("getAllPersonalMessage")]
        public async Task<IActionResult> GetAllPersonalMessagesAsync(string receiverId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var messages = await _messageService.GetAllMessagesAsync(userId, receiverId);
                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get message success",
                    Data = messages
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("getAllConversation")]
        public async Task<IActionResult> GetAllConversationAsync([FromQuery] BaseSearch search)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var conversation = await _conversationService.GetAllConversationAsync(userId, search);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get all conversation success",
                    Data = conversation
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("getFriends")]
        public async Task<IActionResult> GetFriendsAsync([FromQuery] BaseSearch search)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var conversation = await _conversationService.GetFriendsAsync(userId, search);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get all conversation success",
                    Data = conversation
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("getGroupChats")]
        public async Task<IActionResult> GetGroupChatsAsync([FromQuery] BaseSearch search)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var groupChats = await _conversationService.GetGroupChatsAsync(userId, search);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get all group chats success",
                    Data = groupChats
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpGet("getGroupChatMembers")]
        public async Task<IActionResult> GetGroupMembersAsync([FromQuery] BaseSearch search)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var groupMembers = await _conversationService.GetGroupMembersAsync(userId, search);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Get all group chats success",
                    Data = groupMembers
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("createGroupChat")]
        public async Task<IActionResult> CreateGroupChatAsync([FromBody] GroupChatViewModel request)
        {
            try
            {
                if (_groupChatService.ValidateGroupChat(request))
                {
                    return BadRequest(new BaseResponse
                    {
                        Status = 400,
                        Message = "Group Name is exists"
                    });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var groupChat = await _groupChatService.CreateGroupChatAsync(userId, request);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Create Group Chat success",
                    Data = groupChat
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpGet("getAllNotificationMessage")]
        public async Task<IActionResult> GetAllNotificationMessagetAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var notificationMessage = await _chatHubService.GetAllNotificationMessageAsync(userId);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Create Group Chat success",
                    Data = notificationMessage
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
