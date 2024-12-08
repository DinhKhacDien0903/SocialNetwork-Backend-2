using SocialNetwork.DTOs.DTOs;

namespace SocialNetwork.DTOs.Request
{
    public class BaseSearch : BasePagging
    {
        public string? TextSearch { get; set; }

        public string? GroupChatId { get; set; }

        public static BaseSearch Empty => new();
    }
}
