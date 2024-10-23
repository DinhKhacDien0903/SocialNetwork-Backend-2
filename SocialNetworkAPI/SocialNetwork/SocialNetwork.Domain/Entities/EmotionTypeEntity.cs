namespace SocialNetwork.Domain.Entities
{
    public class EmotionTypeEntity
    {
        [Key]
        public Guid EmotionTypeID { get; set; }

        [StringLength(20)]
        [Required]
        public string EmotionName { get; set; } = string.Empty;
    }
}
