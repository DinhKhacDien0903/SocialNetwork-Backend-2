namespace SocialNetwork.Domain.Entities
{
    public class ReactionMessageEntity
    {
        [Key, Column(Order = 0)]
        public Guid ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public Guid MessageID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity Reaction { get; set; } = new ReactionEntity();

        [ForeignKey("MessageID")]
        public MessagesEntity Message { get; set; } = new MessagesEntity();
    }
}
