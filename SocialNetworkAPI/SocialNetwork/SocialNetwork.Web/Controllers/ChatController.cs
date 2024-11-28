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
        public ChatController(
            IMessageService messageService,
            IConversationService conversationService,
            IGroupChatService groupChatService)
        {
            _messageService = messageService;
            _conversationService = conversationService;
            _groupChatService = groupChatService;
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
        public async Task<IActionResult> GetAllConversationAsync([FromQuery] SearchConversation search)
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
        [HttpPost("createGroupChat")]
        public async Task<IActionResult> CreateGroupChatAsync([FromBody] GroupChatViewModel request)
        {
            try
            {
                if(_groupChatService.ValidateGroupChat(request))
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
    }
}
