namespace SocialNetwork.Domain.Entities
{
    public class MessagesEntity:BaseEntity
    {
        [Key]
        public string MessageID { get; set; }
        public string Content { get; set; }

        [Required]
        public string SenderID { get; set; }

        [Required]
        public string ReciverID { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int Symbol { get; set; }

        [ForeignKey("SenderID")]
        public UserEntity? Sender { get; set; }

        [ForeignKey("ReciverID")]
        public UserEntity? Receiver { get; set; }
    }
}
