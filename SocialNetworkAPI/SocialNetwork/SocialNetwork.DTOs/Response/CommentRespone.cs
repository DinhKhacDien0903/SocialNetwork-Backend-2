using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Response
{
    public class CommentRespone
    {
        [Required(ErrorMessage = "UserID is required.")]

        public string? UserID { get; set; }

        public Guid PostID { get; set; }

        public string? Content { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

    }
}
