namespace SocialNetwork.Domain.Entities
{
    public class UserInforEntity : BaseEntity
    {
        [Key]
        public String UserID { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        public bool? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [ForeignKey("UserID")]
        public UserEntity User { get; set; } = new UserEntity();
    }
}
