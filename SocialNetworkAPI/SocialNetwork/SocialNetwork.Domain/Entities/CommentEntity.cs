namespace SocialNetwork.Domain.Entities
{
    public class CommentEntity : BaseEntity
    {
        [Key]
        public Guid CommentID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        public Guid? ParentCommentID { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public bool IsDelete { get; set; } = false;

        [ForeignKey("UserID")]
        public UserEntity User { get; set; }

        [ForeignKey("PostID")]
        public PostEntity Post { get; set; }

        [ForeignKey("ParentCommentID")]
        public CommentEntity? ParentComment { get; set; }

        public ICollection<CommentEntity>? Replies { get; set; } = new List<CommentEntity>();
    }
}
