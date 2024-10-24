namespace SocialNetwork.DTOs.Request
{
    public class SendMessageToPersonRequest
    {
        public string? ReciverId { get; set; }
        public string? Content { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public int Symbol { get; set; }
    }
}
