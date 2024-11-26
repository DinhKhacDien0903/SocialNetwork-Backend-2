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
        
        public string Content { get; set; }

        public List<ImagesOfPostViewModel> Images { get; set; } = new List<ImagesOfPostViewModel>();
        //public List<string> Images { get; set; }

    }
}
