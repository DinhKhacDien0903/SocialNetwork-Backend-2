namespace SocialNetwork.DTOs.ViewModels
{
    public class GroupChatMessageImageViewModel
    {
        public string? GroupChatMessageImageID { get; set; }
        public string? GroupChatMessageID { get; set; }

        public string? UserID { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}
