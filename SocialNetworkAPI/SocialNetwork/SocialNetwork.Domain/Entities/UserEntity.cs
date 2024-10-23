using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Domain.Entities
{
    public class UserEntity : IdentityUser
    {
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        public bool? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
