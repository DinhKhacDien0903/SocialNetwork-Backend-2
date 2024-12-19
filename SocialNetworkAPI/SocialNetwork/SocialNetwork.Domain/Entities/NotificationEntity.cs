namespace SocialNetwork.Domain.Entities
{
    public class NotificationEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        public string? Messeage { get; set; } 

        public string? ReceiverId { get; set; }

        public string? GroupId { get; set; }

        public string SenderId { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        /// <summary>
        /// type = 0 => notification message
        /// type = 1 => notification post
        /// </summary>
        public int Type { get; set; }

        public bool IsDelete { get; set; } = false;

        [ForeignKey("SenderId")]
        public UserEntity? Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public UserEntity? Receiver { get; set; }

        [ForeignKey("GroupId")]
        public GroupChatEntity? GroupChat { get; set; }

    }
}
