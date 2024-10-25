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
        public Guid CommentID { get; set; }

        public string UserID { get; set; }

        public Guid PostID { get; set; }

        public Guid? ParentCommentID { get; set; }

        public string Content { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();

    }
}
