using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DTOs.Request
{
    public class UpdateMessageRequest
    {
        public string? MessageId { get; set; }

        public List<ReactionByUser> ReactionByUser { get; set; } = new List<ReactionByUser>();

        public string? Content { get; set; }

        public string? ReciverId { get; set; }
    }
}   
