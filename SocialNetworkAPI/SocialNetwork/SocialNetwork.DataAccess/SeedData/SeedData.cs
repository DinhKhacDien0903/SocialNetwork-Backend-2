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

                for (int i = 1; i <= 5; i++)
                {
                    var user = new UserEntity
                    {
                        UserName = $"user{i}@test.com",
                        Email = $"user{i}@test.com",
                        FirstName = $"First{i}",
                        LastName = $"Last{i}",
                        IsActive = i <= 8,
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        LastLogin = i <= 8 ? DateTime.UtcNow : (DateTime?)null,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                    if (result.Succeeded)
                    {
                        users.Add(user);
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
                            EmotionName = "HaHa"
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
