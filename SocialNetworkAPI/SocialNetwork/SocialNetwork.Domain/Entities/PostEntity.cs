namespace SocialNetwork.Domain.Entities
{
    public class PostEntity : BaseEntity
    {
        [Key]
        public Guid PostID { get; set; }

        [Required]
        public string UserID { get; set; }

        //[Required]
        public string? Content { get; set; }

        public bool IsDelete { get; set; }

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }

        public ICollection<ImagesOfPostEntity> Images { get; set; } /*= new List<ImagesOfPostEntity>();*/

        public ICollection<ReactionPostEntity> Reactions { get; set; } /*= new List<ReactionPostEntity>();*/

    }
}
