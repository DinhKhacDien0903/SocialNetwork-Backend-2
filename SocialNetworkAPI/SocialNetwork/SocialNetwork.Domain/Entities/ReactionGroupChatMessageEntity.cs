namespace SocialNetwork.Domain.Entities
{
    public class ReactionGroupChatMessageEntity
    {
        [Key, Column(Order = 0)]
        public string ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public string GroupChatMessageID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity? Reaction { get; set; }

        [ForeignKey("GroupChatMessageID")]
        public GroupChatMessageEntity? GroupChatMessage { get; set; }
    }
}
