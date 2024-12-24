using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class FriendRequestViewmodel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public string Message {  get; set; } = string.Empty;

        public int Type {  get; set; }
        public bool IsRead { get; set; }

        public string Id { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; } = string.Empty;

    }
}
