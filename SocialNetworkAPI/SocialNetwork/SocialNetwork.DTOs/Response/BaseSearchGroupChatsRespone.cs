namespace SocialNetwork.DTOs.Response
{
    public class BaseSearchGroupChatsRespone
    {
        public int TotalCount { get; set; } = 0;

        public int TotalPage { get; set; } = 0;

        public IEnumerable<GroupResponse> Groups { get; set; } = new List<GroupResponse>();
    }
}
