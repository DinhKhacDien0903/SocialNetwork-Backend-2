namespace SocialNetwork.DTOs.Response
{
    public class MessagePersonResponse
    {
        public string? MessageID { get; set; }

        public string? Content { get; set; }

        public string? SenderID { get; set; }

        public string? ReciverID { get; set; }

        public int Symbol { get; set; }

        public DateTime? SendDate { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
