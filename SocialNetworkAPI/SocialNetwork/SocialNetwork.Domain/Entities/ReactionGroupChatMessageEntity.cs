namespace SocialNetwork.Domain.Entities
{
    public class ReactionGroupChatMessageEntity
    {
        [Key, Column(Order = 0)]
        public Guid ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public Guid GroupChatMessageID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity Reaction { get; set; } = new ReactionEntity(); 

        [ForeignKey("GroupChatMessageID")]
        public GroupChatMessageEntity GroupChatMessage { get; set; } = new GroupChatMessageEntity();
    }
}
