namespace SocialNetwork.DTOs.Response
{
    public class BaseSearchGroupMemberResponse
    {
        public int TotalCount { get; set; } = 0;

        public int TotalPage { get; set; } = 0;

        public IEnumerable<GroupMemberResponse> GroupMembers { get; set; } = new List<GroupMemberResponse>();
    }
}
