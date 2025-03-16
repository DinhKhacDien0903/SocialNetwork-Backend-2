using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class UserSearchViewModel
    {

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public string Id {  get; set; }

        public bool isRelationShip {  get; set; }

    }
}
