using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialNetwork.Services.AuttoMapper;

namespace SocialNetwork.Web.Dependencies
{
    public static class DependencyInjection
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IRefreshTokenRepository), typeof(RefreshTokenRepository));
            services.AddScoped(typeof(IMessageRepository), typeof(MessageRepository));
            services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
            services.AddScoped(typeof(ICommentRepositories), typeof(CommentRepositories));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IReactionPostRepository), typeof(ReactionPostRepository));
            services.AddScoped(typeof(IReactionRepository), typeof(ReactionRepository));
        }

        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICommentService), typeof(CommentService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IRefreshTokenService), typeof(RefreshTokenService));
            services.AddScoped(typeof(IAuthorService), typeof(AuthorService));
            services.AddScoped(typeof(IChatHubService), typeof(ChatHubService));
            services.AddScoped(typeof(IPostService), typeof(PostService));
            services.AddScoped(typeof(ICommentService), typeof(CommentService));
            services.AddScoped(typeof(IPostHubService), typeof(PostHubService));
            services.AddScoped(typeof(IReactionPostService), typeof(ReactionPostService));
        }


        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddAutoMapper(typeof(AutoMapperConfig));
        }


        public static void AddAuthenticationToken(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(opt =>
            {
                opt.Cookie.Name = "token";
            })

    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? string.Empty)),
            ClockSkew = TimeSpan.Zero
        };

        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };

    }).AddGoogle(options =>
    {
        options.ClientId = configuration["GoogleAuthSetting:ClientID"];
        options.ClientSecret = configuration["GoogleAuthSetting:ClientSecret"];
        options.CallbackPath = "/signin-google";
    });
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SocialNetworkdDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddSwaggerGen_(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialNetwork.Web", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme!",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string [] {}
                    }
                });
            });
        }


        public static void AddSignalR_(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
                    });
            });

            services.AddSignalR();
        }

       
    }
}
