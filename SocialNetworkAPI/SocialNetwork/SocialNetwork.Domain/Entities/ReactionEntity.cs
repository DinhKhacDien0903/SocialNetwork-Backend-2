namespace SocialNetwork.Domain.Entities
{
    public class ReactionEntity : BaseEntity
    {
        [Key]
        public string ReactionID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string EmotionTypeID { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }

        [ForeignKey("EmotionTypeID")]
        public EmotionTypeEntity? EmotionType { get; set; }
    }
}
