namespace SocialNetwork.Domain.Entities
{
    public class ImagesOfPostEntity : BaseEntity
    {
        [Key]
        public string ImagesOfPostID { get; set; }=Guid.NewGuid().ToString();

        [Required]
        public string PostID { get; set; }

        [StringLength(255)]
        public string ImgUrl { get; set; } 

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("PostID")]
        public PostEntity? Post { get; set; } 
    }
}
