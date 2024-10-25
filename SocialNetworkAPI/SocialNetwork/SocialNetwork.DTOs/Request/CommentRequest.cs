using SocialNetwork.DTOs.Response;
using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Request
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "UserID is required.")]
        public string? UserID { get; set; }

        [Required(ErrorMessage = "PostID is required.")]
        public Guid PostID { get; set; }

        public Guid? ParentCommentID { get; set; }

        public string? Content { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public List<CommentRespone> Replies { get; set; } = new List<CommentRespone>();
    }
}
