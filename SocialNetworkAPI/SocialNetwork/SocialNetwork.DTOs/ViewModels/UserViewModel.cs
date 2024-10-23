namespace SocialNetwork.DTOs.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public bool Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
