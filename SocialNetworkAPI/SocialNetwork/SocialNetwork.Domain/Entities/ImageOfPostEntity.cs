namespace SocialNetwork.Domain.Entities
{
    public class ImagesOfPostEntity : BaseEntity
    {
        [Key]
        public Guid ImagesOfPostID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        [StringLength(255)]
        public string ImgUrl { get; set; } /*= string.Empty;*/

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("PostID")]
        public PostEntity? Post { get; set; } /*= new PostEntity();*/
    }
}
