namespace SocialNetwork.Domain.Entities
{
    public class ReactionPostEntity
    {
        [Key, Column(Order = 0)]
        public Guid ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public Guid PostID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity Reaction { get; set; } = new ReactionEntity();

        [ForeignKey("PostID")]
        public PostEntity Post { get; set; } = new PostEntity();
    }
}
