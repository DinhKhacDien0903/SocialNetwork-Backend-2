namespace SocialNetwork.DTOs.Response
{
    public class FriendResponse
    {
        /// <summary>
        /// if the conversation is a private chat, this will be the friend id
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// if the conversation is a private chat, this will be the friend name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// if the conversation is a private chat, this will be the friend name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// avatar of the friend or group
        /// </summary>
        public string AvatarUrl{ get; set; } = string.Empty;
    }
}
