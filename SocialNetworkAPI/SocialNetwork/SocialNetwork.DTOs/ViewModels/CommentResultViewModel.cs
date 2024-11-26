using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DTOs.ViewModels
{
    public class CommentResultViewModel
    {
        public IEnumerable<CommentViewModel>? Comment { get; set; }
        public int NumberOfComment {  get; set; }
    }
}
