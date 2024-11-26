using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DTOs.Response
{
    public class MessagePersonResponse
    {
        public string? MessageID { get; set; }

        public string? Content { get; set; }

        public string? SenderID { get; set; }

        public string? ReciverID { get; set; }

        public int? Symbol { get; set; }

        public List<ReactionByUser>? ReactionByUser { get; set; }

        public int? TotalEmotion { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public bool? IsDelete { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
