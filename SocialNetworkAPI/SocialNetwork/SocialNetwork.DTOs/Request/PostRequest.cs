using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Request
{
    public class PostRequest
    {
        //public string? UserID { get; set; }

        public string? Content { get; set; }

        //public bool IsDelete { get; set; } = false;

        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //public DateTime? UpdatedAt { get; set; }

        public List<ImagesOfPostViewModel> Images { get; set; } = new List<ImagesOfPostViewModel>();
    }
}
