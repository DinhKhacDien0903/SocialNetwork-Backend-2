namespace SocialNetwork.Domain.Entities
{
    public class GroupChatEntity : BaseEntity
    {
        [Key]
        public string GroupChatID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; } = "New Group Chat";

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Avatar { get; set; } = "https://res.cloudinary.com/dlran3qvj/image/upload/v1732701622/file_1732701619587.jpg";

    }
}
