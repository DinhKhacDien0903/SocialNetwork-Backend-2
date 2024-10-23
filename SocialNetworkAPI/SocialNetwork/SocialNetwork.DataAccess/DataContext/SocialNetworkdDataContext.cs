using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using System.Xml.Linq;

namespace SocialNetwork.DataAccess.DataContext
{
    public class SocialNetworkdDataContext : IdentityDbContext<UserEntity>
    {
        public SocialNetworkdDataContext(DbContextOptions<SocialNetworkdDataContext> options) : base(options)
        {
        }

        //public DbSet<UserEntity> Users { get; set; }
        //public DbSet<UserInforEntity> UserInfors { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        //public DbSet<RoleEntity> Roles { get; set; }
        //public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<RelationshipEntity> Relationships { get; set; }
        public DbSet<RequestFriendEntity> RequestFriends { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ImagesOfPostEntity> ImagesOfPosts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<MessagesEntity> Messages { get; set; }
        public DbSet<MessageStatusEntity> MessageStatuses { get; set; }
        public DbSet<MessageImageEntity> MessageImages { get; set; }
        public DbSet<GroupChatEntity> GroupChats { get; set; }
        public DbSet<GroupChatMemberEntity> GroupChatMembers { get; set; }
        public DbSet<GroupChatMessageEntity> GroupChatMessages { get; set; }
        public DbSet<GroupChatMessageStatusEntity> GroupChatMessageStatuses { get; set; }
        public DbSet<GroupChatMessageImageEntity> GroupChatMessageImages { get; set; }
        public DbSet<EmotionTypeEntity> EmotionTypes { get; set; }
        public DbSet<ReactionEntity> Reactions { get; set; }
        public DbSet<ReactionPostEntity> ReactionPosts { get; set; }
        public DbSet<ReactionCommentEntity> ReactionComments { get; set; }
        public DbSet<ReactionMessageEntity> ReactionMessages { get; set; }
        public DbSet<ReactionGroupChatMessageEntity> ReactionGroupChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessagesEntity>()
       .HasOne(m => m.Sender)
       .WithMany() // Nếu không có collection
       .HasForeignKey(m => m.SenderID)
       .OnDelete(DeleteBehavior.Cascade); // Hoặc DeleteBehavior.SetNull nếu muốn đặt giá trị null

            modelBuilder.Entity<MessagesEntity>()
                .HasOne(m => m.Receiver)
                .WithMany() // Nếu không có collection
                .HasForeignKey(m => m.ReciverID) // Đảm bảo tên này khớp với trong code của bạn
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RelationshipEntity>()
         .HasKey(r => new { r.UserID, r.FriendID });

            modelBuilder.Entity<RelationshipEntity>()
                .HasOne(r => r.User)
                .WithMany() // Nếu không có collection
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc SetNull để tránh lỗi

            modelBuilder.Entity<RelationshipEntity>()
                .HasOne(r => r.Friend)
                .WithMany() // Nếu không có collection
                .HasForeignKey(r => r.FriendID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestFriendEntity>()
        .HasKey(rf => rf.RequestFriendID);

            modelBuilder.Entity<RequestFriendEntity>()
                .HasOne(rf => rf.Sender)
                .WithMany() // Nếu không có collection
                .HasForeignKey(rf => rf.SenderID)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc SetNull nếu cần

            modelBuilder.Entity<RequestFriendEntity>()
                .HasOne(rf => rf.Receiver)
                .WithMany() // Nếu không có collection
                .HasForeignKey(rf => rf.ReceiverID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<GroupChatMessageImageEntity>()
        .HasKey(gcmi => gcmi.GroupChatMessageImageID);

            modelBuilder.Entity<GroupChatMessageImageEntity>()
                .HasOne(gcmi => gcmi.GroupChatMessage)
                .WithMany() // Nếu không có collection
                .HasForeignKey(gcmi => gcmi.GroupChatMessageID)
                .OnDelete(DeleteBehavior.Cascade); // Giữ Cascade ở đây nếu cần

            modelBuilder.Entity<GroupChatMessageImageEntity>()
                .HasOne(gcmi => gcmi.User)
                .WithMany() // Nếu không có collection
                .HasForeignKey(gcmi => gcmi.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupChatMessageStatusEntity>()
       .HasKey(gcms => new { gcms.GroupChatMessageID, gcms.UserID });

            modelBuilder.Entity<GroupChatMessageStatusEntity>()
                .HasOne(gcms => gcms.GroupChatMessage)
                .WithMany() // Nếu không có collection
                .HasForeignKey(gcms => gcms.GroupChatMessageID)
                .OnDelete(DeleteBehavior.Cascade); // Giữ Cascade ở đây nếu cần

            modelBuilder.Entity<GroupChatMessageStatusEntity>()
                .HasOne(gcms => gcms.User)
                .WithMany() // Nếu không có collection
                .HasForeignKey(gcms => gcms.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommentEntity>(entity =>
            {
                // Set primary key
                entity.HasKey(c => c.CommentID);

                // Configure foreign key with UserEntity
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserID)
                      .OnDelete(DeleteBehavior.NoAction);  // No cascade delete on User

                // Configure foreign key with PostEntity
                entity.HasOne(c => c.Post)
                      .WithMany()
                      .HasForeignKey(c => c.PostID)
                      .OnDelete(DeleteBehavior.Cascade);  // Allow cascade delete on post deletion

                // Configure self-referencing relationship for ParentComment
                entity.HasOne(c => c.ParentComment)
                      .WithMany(c => c.Replies)
                      .HasForeignKey(c => c.ParentCommentID)
                      .OnDelete(DeleteBehavior.NoAction);  // No cascade delete for parent-child relationship
            });



            modelBuilder.Entity<ReactionGroupChatMessageEntity>(entity =>
            {
                // Composite primary key
                entity.HasKey(rg => new { rg.ReactionID, rg.GroupChatMessageID });

                // Foreign key relationship with ReactionEntity - No cascade
                entity.HasOne(rg => rg.Reaction)
                      .WithMany()
                      .HasForeignKey(rg => rg.ReactionID)
                      .OnDelete(DeleteBehavior.NoAction);  // No action on delete

                // Foreign key relationship with GroupChatMessageEntity - Cascade delete
                entity.HasOne(rg => rg.GroupChatMessage)
                      .WithMany()
                      .HasForeignKey(rg => rg.GroupChatMessageID)
                      .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on GroupChatMessage deletion
            });

            modelBuilder.Entity<ReactionPostEntity>(entity =>
            {
                // Composite primary key
                entity.HasKey(rp => new { rp.ReactionID, rp.PostID });

                // Foreign key relationship with ReactionEntity - No action
                entity.HasOne(rp => rp.Reaction)
                      .WithMany()
                      .HasForeignKey(rp => rp.ReactionID)
                      .OnDelete(DeleteBehavior.NoAction);  // No action on delete

                // Foreign key relationship with PostEntity - Cascade delete
                entity.HasOne(rp => rp.Post)
                      .WithMany()
                      .HasForeignKey(rp => rp.PostID)
                      .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on Post deletion
            });


            modelBuilder.Entity<ReactionMessageEntity>(entity =>
            {
                // Composite primary key
                entity.HasKey(rm => new { rm.ReactionID, rm.MessageID });

                // Foreign key relationship with ReactionEntity - No action
                entity.HasOne(rm => rm.Reaction)
                      .WithMany()
                      .HasForeignKey(rm => rm.ReactionID)
                      .OnDelete(DeleteBehavior.NoAction);  // No action on delete

                // Foreign key relationship with MessageEntity - Cascade delete
                entity.HasOne(rm => rm.Message)
                      .WithMany()
                      .HasForeignKey(rm => rm.MessageID)
                      .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on Message deletion
            });


            modelBuilder.Entity<ReactionCommentEntity>(entity =>
            {
                // Composite primary key
                entity.HasKey(rc => new { rc.ReactionID, rc.CommentID });

                // Foreign key relationship with ReactionEntity - No action
                entity.HasOne(rc => rc.Reaction)
                      .WithMany()
                      .HasForeignKey(rc => rc.ReactionID)
                      .OnDelete(DeleteBehavior.NoAction);  // No action on delete

                // Foreign key relationship with CommentEntity - Cascade delete
                entity.HasOne(rc => rc.Comment)
                      .WithMany()
                      .HasForeignKey(rc => rc.CommentID)
                      .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on Comment deletion
            });






            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            modelBuilder.Entity<RelationshipEntity>()
                .HasKey(r => new { r.UserID, r.FriendID });

            modelBuilder.Entity<GroupChatMemberEntity>()
                .HasKey(gcm => new { gcm.GroupChatID, gcm.UserID });

            modelBuilder.Entity<ReactionPostEntity>()
                .HasKey(rp => new { rp.ReactionID, rp.PostID });

            modelBuilder.Entity<ReactionCommentEntity>()
                .HasKey(rc => new { rc.ReactionID, rc.CommentID });

            modelBuilder.Entity<ReactionMessageEntity>()
                .HasKey(rm => new { rm.ReactionID, rm.MessageID });

            modelBuilder.Entity<ReactionGroupChatMessageEntity>()
                .HasKey(rgcm => new { rgcm.ReactionID, rgcm.GroupChatMessageID });

            modelBuilder.Entity<GroupChatMessageStatusEntity>()
               .HasKey(g => new { g.GroupChatMessageID, g.UserID});

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

        }

    }
}
