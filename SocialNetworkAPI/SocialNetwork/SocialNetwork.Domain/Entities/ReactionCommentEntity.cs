namespace SocialNetwork.Domain.Entities
{
    public class ReactionCommentEntity
    {
        [Key, Column(Order = 0)]
        public Guid ReactionID { get; set; }

        [Key, Column(Order = 1)]
        public Guid CommentID { get; set; }

        [ForeignKey("ReactionID")] 
        public ReactionEntity Reaction { get; set; } = new ReactionEntity(); 

        [ForeignKey("CommentID")] 
        public CommentEntity Comment { get; set; } = new CommentEntity();
    }
}
