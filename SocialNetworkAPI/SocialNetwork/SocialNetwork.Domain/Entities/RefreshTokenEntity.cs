namespace SocialNetwork.Domain.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        [Key]
        public Guid RefreshTokenID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        [StringLength(256)]
        public string Token { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string JwtID { get; set; } = string.Empty;

        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }

        [ForeignKey("UserID")]
        public UserEntity? User { get; set; }
    }
}
