namespace SocialNetwork.DTOs.DTOs
{
    public class BasePagging
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = int.MaxValue;
    }
}
