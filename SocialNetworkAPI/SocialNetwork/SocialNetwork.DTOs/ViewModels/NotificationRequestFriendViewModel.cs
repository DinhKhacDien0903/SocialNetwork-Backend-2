using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class NotificationRequestFriendViewModel
    {
        public string? ReceiverId { get; set; }
        public string SenderId { get; set; }
        public string? Messeage { get; set; }
        public int Type {  get; set; }
        public bool IsRead { get; set; }

    }
}
