using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class NotificationPostViewModel
    {
        //public string PostId { set; get; }
        public string UserId {  get; set; }
        public string Content { set; get; }
        //public bool IsRead { set; get; }
        public string Type { set; get; }

        //public List<string> friendId { set; get; }
        //public DateTime CreatedAt {  set; get; }
    }
}
