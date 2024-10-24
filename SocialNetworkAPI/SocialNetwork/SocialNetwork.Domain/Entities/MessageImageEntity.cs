namespace SocialNetwork.Domain.Entities
{
    public class MessageImageEntity
    {
        [Key]
        public string MessageImageID { get; set; }

        [Required]
        public string MessageID { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("MessageID")]
        public MessagesEntity? Message { get; set; }
    }
}
