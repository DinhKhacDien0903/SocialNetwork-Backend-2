namespace SocialNetwork.DTOs.Request
{
    public class SendMessageToGroupRequest
    {
        public string? GroupChatId { get; set; }
        public string? Content { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public int Symbol { get; set; }
    }
}
