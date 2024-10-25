using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class ReactionPostViewModel
    {
        public Guid ReactionID { get; set; }

        public Guid PostID { get; set; }

        public string UserID { get; set; }

        public Guid EmotionTypeID { get; set; }

        public bool IsDeleted { get; set; }
    }
}
