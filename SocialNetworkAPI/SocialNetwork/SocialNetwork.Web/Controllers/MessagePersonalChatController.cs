using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagePersonalChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagePersonalChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Authorize]
        [HttpGet("getAllMessage")]
        public async Task<IActionResult> GetAllMessagesAsync(string receiverId)
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
    }
}
