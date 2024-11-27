using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DTOs.Request
{
    public class SearchConversation : BasePagging
    {
        public string? TextSearch { get; set; }

        public static SearchConversation Empty => new();
    }
}
