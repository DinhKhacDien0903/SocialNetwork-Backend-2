namespace SocialNetwork.DTOs.DTOs
{
    public class BasePagging
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public bool IsTotalCount { get; set; } = false;
    }
}
