using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain
{
    public class PageResult<tResult>
    {
        public int CurrentPage {  get; set; }
        public int TotalCount { get; set; }
        public List<tResult> Data {  get; set; }
        public int AmountInPage=> Data.Count;
    }
}
