namespace SocialNetwork.DTOs.Response
{
    public class ReactionMessageResponse
    {
        public string? MessageId { get; set; }

        public string? EmotionType { get; set; }

        public string? ReciverId { get; set; }

        public string? SenderId { get; set; }

        public DateTime? ReactionAt { get; set; }
    }
}
