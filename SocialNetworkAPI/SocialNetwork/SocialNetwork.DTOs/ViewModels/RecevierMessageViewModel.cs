namespace SocialNetwork.DTOs.ViewModels
{
    public class RecevierMessageViewModel
    {
        public string? MessageID { get; set; }

        public string? SenderID { get; set; }

        public string? RecevierID { get; set; }

        public string? Message { get; set; }

        public DateTime SendDate { get; set; }
    }
}
