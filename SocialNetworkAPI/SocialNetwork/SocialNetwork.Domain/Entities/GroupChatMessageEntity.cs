namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMessageEntity: BaseEntity
    {
        [Key]
        public Guid GroupChatMessageID { get; set; }

        [Required]
        public Guid GroupChatID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public GroupChatEntity GroupChat { get; set; } = new GroupChatEntity(); 

        [ForeignKey("UserID")]
        public UserEntity User { get; set; } = new UserEntity(); 
    }
}
