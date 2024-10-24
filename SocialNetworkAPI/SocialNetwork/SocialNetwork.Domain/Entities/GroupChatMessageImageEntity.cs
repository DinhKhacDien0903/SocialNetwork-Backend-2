namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMessageImageEntity
    {
        [Key]
        public string GroupChatMessageImageID { get; set; }

        [Required]
        public string GroupChatMessageID { get; set; }

        [Required]
        public string UserID { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("GroupChatMessageID")]
        public GroupChatMessageEntity? GroupChatMessage { get; set; } 

        [ForeignKey("UserID")] 
        public UserEntity? User { get; set; }
    }
}
