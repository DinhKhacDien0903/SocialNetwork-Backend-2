namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMessageEntity: BaseEntity
    {
        [Key]
        public string GroupChatMessageID { get; set; }

        [Required]
        public string GroupChatID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public GroupChatEntity? GroupChat { get; set; }

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }
    }
}
