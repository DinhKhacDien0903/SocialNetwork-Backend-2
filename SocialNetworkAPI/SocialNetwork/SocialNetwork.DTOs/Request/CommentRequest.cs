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
        public string PostID { get; set; }

        public string? ParentCommentID { get; set; }

        public string? Content { get; set; }
    }
}
