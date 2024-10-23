using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class ImagesOfPostViewModel
    {
        public Guid PostID { get; set; }
        public string ImgUrl { get; set; }
        public bool IsDeleted { get; set; }=false;
       
    }
}
