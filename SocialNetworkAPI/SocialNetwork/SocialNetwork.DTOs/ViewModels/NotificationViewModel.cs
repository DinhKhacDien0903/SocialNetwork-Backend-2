namespace SocialNetwork.DTOs.ViewModels
{
    public class NotificationViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string? Messeage { get; set; }

        public string? ReceiverId { get; set; }

        public string? GroupId { get; set; }

        public string SenderId { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public int Type { get; set; } = 0;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
