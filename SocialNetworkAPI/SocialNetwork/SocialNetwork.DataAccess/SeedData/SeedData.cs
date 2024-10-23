using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.DataAccess.SeedData
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserEntity> userManager)
        {
            var context = serviceProvider.GetRequiredService<SocialNetworkdDataContext>();

            if (!userManager.Users.Any())
            {
                var users = new List<UserEntity>();

                // Seed 10 users
                for (int i = 1; i <= 5; i++)
                {
                    var user = new UserEntity
                    {
                        UserName = $"user{i}@test.com",
                        Email = $"user{i}@test.com",
                        FirstName = $"First{i}",
                        LastName = $"Last{i}",
                        IsActive = i <= 8, // 8 users online, 2 users offline
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        LastLogin = i <= 8 ? DateTime.UtcNow : (DateTime?)null, // Last login for online users
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                    }
                }

                // Save users to the database
                await context.SaveChangesAsync(); // Ensure users are saved before creating relationships

                users = await context.Users.Select(x => x).ToListAsync();
                // Seed relationships for each user
                if (!context.Set<RelationshipEntity>().Any())
                {
                    var relationships = new List<RelationshipEntity>();

                    foreach (var user in users)
                    {
                        // Tạo 5 mối quan hệ bạn bè
                        var friends = users.Where(u => u.Id != user.Id).Take(5).ToList();

                        foreach (var friend in friends)
                        {
                            // Thêm quan hệ 2 chiều giữa user và bạn
                            //relationships.Add(new RelationshipEntity
                            //{
                            //    UserID = user.Id,
                            //    FriendID = friend.Id,
                            //    IsDeleted = false
                            //});

                            //relationships.Add(new RelationshipEntity
                            //{
                            //    UserID = friend.Id,
                            //    FriendID = user.Id,
                            //    IsDeleted = false
                            //});
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
                    // Add relationships to the context and save to the database
                    //context.Relationships.AddRange(relationships);
                    //await context.SaveChangesAsync(); // Ensure relationships are saved before creating messages
                }

                // Seed messages between friends
                if (!context.Set<MessagesEntity>().Any())
                {
                    var messages = new List<MessagesEntity>();

                    foreach (var relationship in context.Set<RelationshipEntity>())
                    {
                        var sender = users.FirstOrDefault(u => u.Id == relationship.UserID);
                        var receiver = users.FirstOrDefault(u => u.Id == relationship.FriendID);

                        if (sender != null && receiver != null)
                        {
                            // Tạo tin nhắn từ sender đến receiver
                            messages.Add(new MessagesEntity
                            {
                                MessageID = Guid.NewGuid(),
                                Content = $"Hello from {sender.UserName} to {receiver.UserName}",
                                SenderID = sender.Id,
                                ReciverID = receiver.Id,
                                IsDeleted = false,
                                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
                            });

                            // Tạo tin nhắn phản hồi từ receiver đến sender
                            messages.Add(new MessagesEntity
                            {
                                MessageID = Guid.NewGuid(),
                                Content = $"Reply from {receiver.UserName} to {sender.UserName}",
                                SenderID = receiver.Id,
                                ReciverID = sender.Id,
                                IsDeleted = false,
                                CreatedAt = DateTime.UtcNow.AddMinutes(-5)
                            });
                        }
                    }

                    // Add messages to the context and save to the database
                    context.AddRange(messages);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
