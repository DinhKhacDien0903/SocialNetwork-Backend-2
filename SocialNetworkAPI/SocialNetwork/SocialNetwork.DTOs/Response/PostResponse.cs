using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Response
{
    public class PostResponse
    {
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string? Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public List<ImagesOfPostViewModel> Images { get; set; } = new List<ImagesOfPostViewModel>();
    }
}
