namespace SocialNetwork.DTOs.ViewModels
{
    public class GroupChatViewModel
    {
        /// <summary>
        /// Id of the group chat
        /// </summary>
        public string GroupChatID { get; set; } = string.Empty;

        /// <summary>
        /// Name of the group chat
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the group chat
        /// </summary>
        public string? Description { get; set; } = "This is a new group chat";

        /// <summary>
        /// List member id of the group chat
        /// </summary>
        public List<string> Members { get; set; } = new List<string>();

        /// <summary>
        /// avatar url of the group chat
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Created time of the group chat
        /// </summary>
        public DateTime? CreatedAt { get; set; } 
    }
}
