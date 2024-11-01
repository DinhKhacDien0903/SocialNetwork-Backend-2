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

        public string UserFirstName {  get; set; }

        public string UserLastName {  get; set; }

        public string AvatarUser { get; set; }

        public string CurrentEmotionId { get; set; }

        public string CurrentEmotionName { get; set; }

        public List<ReactionPostViewModel> Reactions {  get; set; }=new List<ReactionPostViewModel>();

        public List<ImagesOfPostViewModel> Images { get; set; } = new List<ImagesOfPostViewModel>();
    }

}