using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class AdminBrowsePostViewModel
    {
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string? Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public List<ImagesOfPostViewModel>? Images { get; set; } = new List<ImagesOfPostViewModel>();
    }
}
