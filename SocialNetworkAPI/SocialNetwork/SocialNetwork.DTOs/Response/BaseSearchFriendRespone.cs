namespace SocialNetwork.DTOs.Response
{
    public class BaseSearchFriendRespone
    {
        public int TotalCount { get; set; } = 0;

        public int TotalPage { get; set; } = 0;

        public IEnumerable<FriendResponse> Friends { get; set; } = new List<FriendResponse>();
    }
}
