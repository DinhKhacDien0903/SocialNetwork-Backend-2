namespace SocialNetwork.DTOs.Request
{
    public class UpdateMessageRequest
    {
        public string? MessageId { get; set; }

        public string? Content { get; set; }

        public string? ReciverId { get; set; }
    }
}   
