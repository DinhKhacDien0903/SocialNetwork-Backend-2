namespace SocialNetwork.Domain.Entities
{
    public class RequestFriendEntity : BaseEntity
    {
        [Key]
        public Guid RequestFriendID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string FriendID { get; set; }


        public FriendshipStatus Status { get; set; }

        [ForeignKey("UserID")]
        public UserEntity User { get; set; }

        [ForeignKey("FriendID")]
        public UserEntity Friend { get; set; }
    }
}
