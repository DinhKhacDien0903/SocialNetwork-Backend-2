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

        //public string ReactionID { get; set; }

        public string UserID { get; set; }

        public string EmotionTypeID { get; set; }

        public string EmotionName {  get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AvatarUrl { get; set; }

        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
