using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.DTOs.Request
{
    public class MessageGroupViewModel
    {
        public string? GroupChatMessageID { get; set; }

        public string GroupChatID { get; set; } = string.Empty;

        public string UserID { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
