namespace SocialNetwork.DTOs.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? AvatarUrl { get; set; }

        public bool? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool? isPrivate { get; set; } = false;

        public string? DateOfBirthFormatted => DateOfBirth?.ToString("yyyy-MM-dd");

        public int? totalOfFirend { get; set; } = 0;
    }
}
