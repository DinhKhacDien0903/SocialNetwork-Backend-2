namespace SocialNetwork.DTOs.ViewModels
{
    public class MessageViewModel
    {
        public string Content { get; set; } = string.Empty;

        public string SenderID { get; set; } = string.Empty;

        public string ReciverID { get; set; } = string.Empty;

        public int Symbol { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
