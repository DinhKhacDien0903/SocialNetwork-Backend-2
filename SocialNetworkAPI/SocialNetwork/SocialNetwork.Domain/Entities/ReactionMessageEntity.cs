namespace SocialNetwork.Domain.Entities
{
    public class ReactionMessageEntity
    {
        [Key, Column(Order = 0)]
        public string ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public string MessageID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity? Reaction { get; set; }

        [ForeignKey("MessageID")]
        public MessagesEntity? Message { get; set; }
    }
}
