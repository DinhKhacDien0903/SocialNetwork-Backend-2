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
        public string PostID { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
