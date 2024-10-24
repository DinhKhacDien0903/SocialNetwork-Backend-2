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

    }
}
