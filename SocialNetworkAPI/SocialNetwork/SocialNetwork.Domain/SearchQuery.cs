using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain
{
    public class SearchQuery
    {
        private int pageIndex;
        public int PageIndex
        {
            get { return pageIndex; }

            set
            {
                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                else
                {
                    pageIndex = value;
                }
            }
        }
        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value <= 0)
                {
                    pageSize = 9;
                }
                else
                {
                    pageSize = value;
                }
            }
        }
        public string keyWord {  get; set; }

        public int SkipNo => (PageIndex - 1) * pageSize;

        public int TakeNo => pageSize;
    }
}
