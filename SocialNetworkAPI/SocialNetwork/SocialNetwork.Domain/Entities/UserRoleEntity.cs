using System.Data;

namespace SocialNetwork.Domain.Entities
{
    public class UserRoleEntity
    {
        [Key, Column(Order = 0)]
        public string UserID { get; set; }

        [Key, Column(Order = 1)]
        public Guid RoleID { get; set; }

        [ForeignKey("UserID")]
        public UserEntity User { get; set; } = new UserEntity();

        [ForeignKey("RoleID")]
        public RoleEntity Role { get; set; } = new RoleEntity();
    }
}
