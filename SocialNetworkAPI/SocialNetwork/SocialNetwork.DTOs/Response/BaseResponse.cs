namespace SocialNetwork.DTOs.Response
{
    public class BaseResponse
    {
        public int Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public object Data { get; set; } = new object();
    }
}
