namespace SocialNetwork.Domain.Entities
{
    public class MessageStatusEntity 
    {
        [Key]
        public string MessageID { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsRecived { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        [ForeignKey("MessageID")]
        public MessagesEntity? Message { get; set; }
    }
}
