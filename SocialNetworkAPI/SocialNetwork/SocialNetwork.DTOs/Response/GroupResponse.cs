namespace SocialNetwork.DTOs.Response
{
    public class GroupResponse
    {
        /// <summary>
        /// if the conversation is a private chat, this will be the friend id
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// the name of group
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// avatar of the friend or group
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
