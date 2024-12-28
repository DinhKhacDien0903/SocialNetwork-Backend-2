namespace SocialNetwork.DTOs.ViewModels
{
    public class GroupChatMessageViewModel
    {
        public string? GroupChatMessageID { get; set; }

        public string? GroupChatID { get; set; }

        public string? UserID { get; set; }

        public string? Content { get; set; } = string.Empty;

        public bool? IsDeleted { get; set; } = false;

        public int? Symbol { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<string> Images { get; set; } = new List<string>();

        public static GroupChatMessageViewModel Empty => new();
    }
}
