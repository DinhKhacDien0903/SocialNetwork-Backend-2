namespace SocialNetwork.Domain.Entities
{
    public class RequestFriendEntity : BaseEntity
    {
        [Key]
        public Guid RequestFriendID { get; set; }

        [Required]
        public string SenderID { get; set; }

        [Required]
        public string ReceiverID { get; set; }

        public bool IsPending { get; set; } = true;
        public bool IsAccepted { get; set; } = false;
        public bool IsRejected { get; set; } = false;

        [ForeignKey("SenderID")]
        public UserEntity Sender { get; set; } = new UserEntity();

        [ForeignKey("ReceiverID")]
        public UserEntity Receiver { get; set; } = new UserEntity();
    }
}
