using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.DTOs.ViewModels
{
    public class PostViewModel
    {
        public Guid PostID { get; set; }

        [Required(ErrorMessage = "UserID is required.")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        public bool IsDelete { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public List<ImagesOfPostViewModel> Images { get; set; } = new List<ImagesOfPostViewModel>();
    }

}