namespace SocialNetwork.DTOs.Request
{
    public class ReactionMessageRequest
    {
        public string? MessageId { get; set; }

        public string? EmotionType { get; set; }

        public string? ReciverId { get; set; }

    }
}
