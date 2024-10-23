namespace SocialNetwork.Domain.Entities
{
    public  class RelationshipEntity
    {
        [Key, Column(Order = 0)]
        public string UserID { get; set; }

        [Key, Column(Order = 1)]
        public string FriendID { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        [ForeignKey("UserID")]
        public UserEntity User { get; set; }

        [ForeignKey("FriendID")]
        public UserEntity Friend { get; set; }
    }
}
