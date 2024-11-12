namespace SocialNetwork.Domain.Entities
{
    public class CommentEntity :BaseEntity
    {
        [Key]
        public string CommentID { get; set; }  

        [Required]
        public string UserID { get; set; }

        [Required]
        public string PostID { get; set; }

        public string? ParentCommentID { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsDelete { get; set; } = false;

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }  

        [ForeignKey("PostID")]
        public PostEntity? Post { get; set; } 

        [ForeignKey("ParentCommentID")]
        public CommentEntity? ParentComment { get; set; } 

        public ICollection<CommentEntity> Children {  get; set; }=new List<CommentEntity>();
    }
}
