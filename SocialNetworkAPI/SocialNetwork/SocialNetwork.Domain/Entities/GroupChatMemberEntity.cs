namespace SocialNetwork.Domain.Entities
{
    public class GroupChatMemberEntity
    {
        [Key, Column(Order = 0)]
        public string GroupChatID { get; set; }

        [Key, Column(Order = 1)]
        public string UserID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime JoinAt { get; set; }

        public bool IsAdmin { get; set; } = false;
        public bool IsLeaved { get; set; } = false;

        [ForeignKey("GroupChatID")]
        public GroupChatEntity? GroupChat { get; set; }

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }
    }
}
