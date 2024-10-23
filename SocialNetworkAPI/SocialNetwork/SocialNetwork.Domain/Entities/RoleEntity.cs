namespace SocialNetwork.Domain.Entities
{
    public class RoleEntity
    {
        [Key]
        public Guid RoleID { get; set; }

        [StringLength(50)]
        public string? RoleName { get; set; }
    }
}
