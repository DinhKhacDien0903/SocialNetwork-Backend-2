namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMessageStatusEntity
    {
        [Key, Column(Order = 0)]
        public Guid GroupChatMessageID { get; set; }

        [Key, Column(Order = 1)]
        public string UserID { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsRecived { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        [ForeignKey("GroupChatMessageID")]
        public GroupChatMessageEntity GroupChatMessage { get; set; } = new GroupChatMessageEntity();

        [ForeignKey("UserID")]
        public UserEntity User { get; set; } = new UserEntity();
    }
}
