using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SocialNetwork.DataAccess.SeedData
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserEntity> userManager)
        {
            var context = serviceProvider.GetRequiredService<SocialNetworkdDataContext>();

            // Kiểm tra và tạo người dùng
            if (!userManager.Users.Any())
            {
                var users = new List<UserEntity>();

                // Seed 5 người dùng
                for (int i = 1; i <= 5; i++)
                {
                    var user = new UserEntity
                    {
                        UserName = $"user{i}@test.com",
                        Email = $"user{i}@test.com",
                        FirstName = $"First{i}",
                        LastName = $"Last{i}",
                        IsActive = i <= 4, // 4 người dùng online, 1 người offline
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        LastLogin = i <= 4 ? DateTime.UtcNow : (DateTime?)null,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                    }
                }

                // Lưu người dùng vào cơ sở dữ liệu
                await context.SaveChangesAsync();

                users = await context.Users.Select(x => x).ToListAsync();

                // Seed các bài viết
                if (!context.Set<PostEntity>().Any())
                {
                    var posts = new List<PostEntity>();
                    var reactionPosts = new List<ReactionPostEntity>();

                    foreach (var user in users)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            var post = new PostEntity
                            {
                                PostID = Guid.NewGuid().ToString(),
                                UserID = user.Id,
                                Content = $"This is post number {j} by {user.UserName}",
                                IsDelete = false,
                                Images = new List<ImagesOfPostEntity>()
                            };

                            // Thêm hình ảnh cho mỗi bài đăng
                            for (int k = 1; k <= 2; k++)
                            {
                                post.Images.Add(new ImagesOfPostEntity
                                {
                                    ImagesOfPostID = Guid.NewGuid().ToString(),
                                    PostID = post.PostID,
                                    ImgUrl = $"https://example.com/image{k}_{post.PostID}.jpg",
                                    IsDeleted = false
                                });
                            }

                            posts.Add(post);

                            // Tạo và lưu phản ứng cho bài đăng
                            foreach (var emotionType in context.Set<EmotionTypeEntity>().ToList())
                            {
                                var reaction = new ReactionEntity
                                {
                                    ReactionID = Guid.NewGuid().ToString(),
                                    UserID = user.Id,
                                    EmotionTypeID = emotionType.EmotionTypeID,
                                    IsDeleted = false
                                };

                                await context.Set<ReactionEntity>().AddAsync(reaction);
                                await context.SaveChangesAsync(); // Lưu lại để ReactionID có trong cơ sở dữ liệu

                                reactionPosts.Add(new ReactionPostEntity
                                {
                                    ReactionID = reaction.ReactionID,
                                    PostID = post.PostID
                                });
                            }
                        }
                    }

                    // Lưu các bài đăng và liên kết ReactionPostEntity
                    await context.Set<PostEntity>().AddRangeAsync(posts);
                    await context.Set<ReactionPostEntity>().AddRangeAsync(reactionPosts);
                    await context.SaveChangesAsync();
                }


                // Seed bình luận
                if (!context.Set<CommentEntity>().Any())
                {
                    var comments = new List<CommentEntity>();
                    var posts = await context.Set<PostEntity>().ToListAsync();

                    foreach (var post in posts)
                    {
                        foreach (var user in users)
                        {
                            var comment = new CommentEntity
                            {
                                CommentID = Guid.NewGuid().ToString(),
                                UserID = user.Id,
                                PostID = post.PostID,
                                Content = $"This is a comment by {user.UserName} on post {post.PostID}",
                                IsDelete = false,
                                Replies = new List<CommentEntity>()
                            };

                            comments.Add(comment);

                            // Tạo trả lời cho bình luận chính
                            for (int i = 1; i <= 2; i++)
                            {
                                var reply = new CommentEntity
                                {
                                    CommentID = Guid.NewGuid().ToString(),
                                    UserID = user.Id,
                                    PostID = post.PostID,
                                    ParentCommentID = comment.CommentID,
                                    Content = $"This is reply {i} to comment {comment.CommentID} by {user.UserName}",
                                    IsDelete = false
                                };

                                comment.Replies.Add(reply);
                                comments.Add(reply);
                            }
                        }
                    }

                    await context.Set<CommentEntity>().AddRangeAsync(comments);
                    await context.SaveChangesAsync();
                }

                // Seed phản ứng cho bình luận
                if (!context.Set<ReactionCommentEntity>().Any())
                {
                    var reactionComments = new List<ReactionCommentEntity>();
                    var comments = await context.Set<CommentEntity>().ToListAsync();

                    foreach (var comment in comments)
                    {
                        foreach (var user in users)
                        {
                            foreach (var emotionType in await context.Set<EmotionTypeEntity>().ToListAsync())
                            {
                                var reaction = new ReactionEntity
                                {
                                    ReactionID = Guid.NewGuid().ToString(),
                                    UserID = user.Id,
                                    EmotionTypeID = emotionType.EmotionTypeID,
                                    IsDeleted = false
                                };

                                await context.Set<ReactionEntity>().AddAsync(reaction);

                                reactionComments.Add(new ReactionCommentEntity
                                {
                                    ReactionID = reaction.ReactionID,
                                    CommentID = comment.CommentID,
                                    Reaction = reaction,
                                    Comment = comment
                                });
                            }
                        }
                    }

                    await context.Set<ReactionCommentEntity>().AddRangeAsync(reactionComments);
                    await context.SaveChangesAsync();
                }

                // Seed loại cảm xúc

            }
            if (!context.Set<EmotionTypeEntity>().Any())
            {
                    var emotionTypes = new List<EmotionTypeEntity>
                        {
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Like" },
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Love" },
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Haha" },
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Wow" },
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Sad" },
                            new EmotionTypeEntity { EmotionTypeID = Guid.NewGuid().ToString(), EmotionName = "Angry" }
                        };

                    await context.Set<EmotionTypeEntity>().AddRangeAsync(emotionTypes);
                    await context.SaveChangesAsync();
            }
        }
    }
}
