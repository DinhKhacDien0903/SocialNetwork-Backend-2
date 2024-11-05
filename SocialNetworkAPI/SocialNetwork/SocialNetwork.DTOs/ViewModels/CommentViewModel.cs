using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class CommentViewModel
    {
        public string CommentID { get; set; }

        public string PostID { get; set; }

        public string? ParentCommentID { get; set; }

        public string Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
