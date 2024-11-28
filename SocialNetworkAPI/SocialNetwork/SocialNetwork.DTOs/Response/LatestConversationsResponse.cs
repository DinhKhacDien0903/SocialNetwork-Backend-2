namespace SocialNetwork.DTOs.Response
{
    public class LatestConversationsResponse
    {
        /// <summary>
        /// This is Id from sender messages
        /// </summary>
        public required string SenderId { get; set; }
        /// <summary>
        /// If the conversation is a group chat, this will be the group id
        /// </summary>
        public string GroupId { get; set; } = string.Empty;

        /// <summary>
        /// if the conversation is a private chat, this will be the friend id
        /// </summary>
        public string FriendId { get; set; } = string.Empty;

        /// <summary>
        /// if the conversation is a group chat, this will be the group name
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

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
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// if the conversation is a group chat, this will be the group admin id
        /// </summary>
        public string AdministratorId { get; set; } = string.Empty;

        /// <summary>
        /// If the conversation is a group chat return true, else return false
        /// </summary>
        public bool IsGroupChat { get; set; }

        /// <summary>
        /// The last message in the conversation
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public DateTime? LastMessageCreated { get; set; }
    }
}
