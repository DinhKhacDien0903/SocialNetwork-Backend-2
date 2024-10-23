namespace SocialNetwork.Domain.Entities
{
    public class MessageImageEntity
    {
        [Key]
        public Guid MessageImageID { get; set; }

        [Required]
        public Guid MessageID { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("MessageID")]
        public MessagesEntity Message { get; set; } = new MessagesEntity();
    }
}
