using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Request
{
    public class ReactionRequest
    {
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string EmotionTypeID { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
