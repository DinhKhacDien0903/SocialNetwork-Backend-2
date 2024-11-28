namespace SocialNetwork.DTOs.Response
{
    public class BaseSearchConversationResponse
    {
        public int TotalCount { get; set; } = 0;

        public int TotalPage { get; set; } = 0;

        public IEnumerable<LatestConversationsResponse> Conversations { get; set; } = new List<LatestConversationsResponse>();
    }
}
