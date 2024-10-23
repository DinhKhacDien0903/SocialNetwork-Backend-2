namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMessageImageEntity
    {
        [Key]
        public Guid GroupChatMessageImageID { get; set; }

        [Required]
        public Guid GroupChatMessageID { get; set; }

        [Required]
        public string UserID { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("GroupChatMessageID")]
        public GroupChatMessageEntity GroupChatMessage { get; set; } = new GroupChatMessageEntity(); 

        [ForeignKey("UserID")] 
        public UserEntity User { get; set; } = new UserEntity(); 
    }
}
