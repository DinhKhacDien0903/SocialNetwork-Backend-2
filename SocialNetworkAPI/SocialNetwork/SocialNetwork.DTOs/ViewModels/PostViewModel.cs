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
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string? Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AvatarUrl { get; set; }

        //public string? EmotionTypeID { get; set; }

        //public string? EmotionName { get; set; }

        public EmotionViewModel UserReaction {  get; set; }

        public List<ReactionPostViewModel>? Reactions {  get; set; }=new List<ReactionPostViewModel>();

        public List<ImagesOfPostViewModel>? Images { get; set; } = new List<ImagesOfPostViewModel>();
    }

}