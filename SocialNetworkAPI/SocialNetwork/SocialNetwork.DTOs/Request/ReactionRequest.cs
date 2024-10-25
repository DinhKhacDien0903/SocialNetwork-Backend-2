using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.Request
{
    public class ReactionRequest
    {
        public Guid PostID { get; set; }

        public string UserID { get; set; }

        public Guid EmotionTypeID { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
