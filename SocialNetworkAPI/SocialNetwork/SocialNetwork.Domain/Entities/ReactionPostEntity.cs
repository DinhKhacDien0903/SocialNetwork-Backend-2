namespace SocialNetwork.Domain.Entities
{
    public class ReactionPostEntity
    {
        [Key, Column(Order = 0)]
        public string ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public string PostID { get; set; }

        [ForeignKey("ReactionID")]
        public ReactionEntity? Reaction { get; set; }

        [ForeignKey("PostID")]
        public PostEntity? Post { get; set; }
    }
}
