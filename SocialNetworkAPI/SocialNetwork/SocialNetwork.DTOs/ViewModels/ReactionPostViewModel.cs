using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class ReactionPostViewModel
    {
        public string ReactionID { get; set; }

        public string UserID { get; set; }

        public string EmotionTypeID { get; set; }

        public string EmotionName { get; set; }
    }
}
