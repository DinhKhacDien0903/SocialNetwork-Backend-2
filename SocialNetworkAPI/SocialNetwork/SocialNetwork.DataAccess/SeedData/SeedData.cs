using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.DataAccess.SeedData
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserEntity> userManager)
        {
            var context = serviceProvider.GetRequiredService<SocialNetworkdDataContext>();

            var role = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.Roles.Any())
            {
                var roles = new[] { "Admin", "User" };

                foreach (var item in roles)
                {
                    if (!await role.RoleExistsAsync(item))
                        await role.CreateAsync(new IdentityRole(item));
                }
            }

            if (!userManager.Users.Any())
            {
                var users = new List<UserEntity>();

                for (int i = 1; i <= 5; i++)
                {
                    var user = new UserEntity
                    {
                        UserName = $"user{i}@test.com",
                        Email = $"user{i}@test.com",
                        FirstName = $"First{i}",
                        LastName = $"Last{i}",
                        IsActive = i <= 4,
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        LastLogin = i <= 4 ? DateTime.UtcNow : (DateTime?)null,
                        Gender = false,
                        EmailConfirmed = true,
                        AvatarUrl = "https://res.cloudinary.com/dlran3qvj/image/upload/v1732701622/file_1732701619587.jpg"
                    };

                    var result = await userManager.CreateAsync(user, "ABCd123!@#");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }

                await context.SaveChangesAsync();
                users = await context.Users.Select(x => x).ToListAsync();

                if (!context.Set<RelationshipEntity>().Any())
                {
                    var relationships = new List<RelationshipEntity>();

                    foreach (var user in users)
                    {
                        var friends = users.Where(u => u.Id != user.Id).Take(5).ToList();

                        foreach (var friend in friends)
                        {
                            var x = new RelationshipEntity
                            {
                                UserID = user.Id,
                                FriendID = friend.Id,
                                IsDeleted = false
                            };

                            context.Relationships.Add(x);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                //post initial
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
                                Children = new List<CommentEntity>()
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

                                comment.Children.Add(reply);
                                comments.Add(reply);
                            }
                        }
                    }

                    await context.Set<CommentEntity>().AddRangeAsync(comments);
                    await context.SaveChangesAsync();
                }


                if (!context.Set<MessagesEntity>().Any())
                {
                    var messages = new List<MessagesEntity>();

                    foreach (var relationship in context.Set<RelationshipEntity>())
                    {
                        var sender = users.FirstOrDefault(u => u.Id == relationship.UserID);
                        var receiver = users.FirstOrDefault(u => u.Id == relationship.FriendID);

                        if (sender != null && receiver != null)
                        {
                            var sendDate = DateTime.UtcNow.AddMinutes(-10);
                            var receiverDate = DateTime.UtcNow.AddMinutes(-5);
                            messages.Add(new MessagesEntity
                            {
                                MessageID = Guid.NewGuid().ToString(),
                                Content = $"Hello from {sender.UserName} to {receiver.UserName}",
                                SenderID = sender.Id,
                                ReciverID = receiver.Id,
                                IsDeleted = false,
                                CreatedAt = sendDate,
                                UpdatedAt = sendDate
                            });

                            messages.Add(new MessagesEntity
                            {
                                MessageID = Guid.NewGuid().ToString(),
                                Content = $"Reply from {receiver.UserName} to {sender.UserName}",
                                SenderID = receiver.Id,
                                ReciverID = sender.Id,
                                IsDeleted = false,
                                CreatedAt = receiverDate,
                                UpdatedAt = receiverDate
                            });
                        }
                    }

                    context.AddRange(messages);
                    await context.SaveChangesAsync();
                }
            }
            if (!await context.EmotionTypes.AnyAsync())
            {
                var emotionTypes = new List<EmotionTypeEntity>()
                    {
                        new EmotionTypeEntity
                        {
                            EmotionTypeID = "0",
                            EmotionName = "Like"
                        },
                         new EmotionTypeEntity
                        {
                            EmotionTypeID = "1",
                            EmotionName = "Love"
                        },
                        new EmotionTypeEntity
                        {
                            EmotionTypeID = "2",
                            EmotionName = "Haha"
                        },
                        new EmotionTypeEntity
                        {
                            EmotionTypeID = "3",
                            EmotionName = "Wow"
                        },
                        new EmotionTypeEntity
                        {
                            EmotionTypeID = "4",
                            EmotionName = "Sad"
                        },
                        new EmotionTypeEntity
                        {
                            EmotionTypeID = "5",
                            EmotionName = "Angry"
                        },

                    };

                context.EmotionTypes.AddRange(emotionTypes);

                await context.SaveChangesAsync();
            }
        }
    }
}