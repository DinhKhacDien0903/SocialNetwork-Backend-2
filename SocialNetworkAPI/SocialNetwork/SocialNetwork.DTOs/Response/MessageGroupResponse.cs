using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DTOs.Response
{
    public class MessageGroupResponse
    {
        public string? GroupChatMessageID { get; set; }

        public string? Content { get; set; }

        public string? UserID { get; set; }

        public string? SenderAvatar { get; set; }

        public string? GroupChatID { get; set; }

        public int? Symbol { get; set; }

        public List<ReactionByUser>? ReactionByUser { get; set; }

        public int? TotalEmotion { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public bool? IsDeleted { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
