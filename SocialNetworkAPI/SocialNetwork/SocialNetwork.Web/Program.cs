using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialNetwork.DataAccess.SeedData;
using SocialNetwork.Domain.Entities;
using SocialNetwork.DTOs.Authorize;
using SocialNetwork.Helpers.Hubs;
using SocialNetwork.Services.AuttoMapper;
using SocialNetwork.Web.Hubs;
using SocialNetwork.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<SocialNetworkdDataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<SocialNetworkdDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepositories, CommentRepositories>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddScoped(typeof(IPostHubService), typeof(PostHubService));
builder.Services.AddScoped(typeof(IReactionPostService), typeof(ReactionPostService));
builder.Services.AddScoped(typeof(IReactionPostRepository), typeof(ReactionPostRepository));
builder.Services.AddScoped(typeof(IReactionRepository), typeof(ReactionRepository));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IRefreshTokenRepository),typeof(RefreshTokenRepository));
builder.Services.AddScoped(typeof(IMessageRepository),typeof(MessageRepository));
builder.Services.AddScoped(typeof(IRelationshipRepository),typeof(RelationshipRepository));

builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IRefreshTokenService), typeof(RefreshTokenService));
builder.Services.AddScoped(typeof(IAuthorService), typeof(AuthorService));
builder.Services.AddScoped(typeof(IChatHubService), typeof(ChatHubService));
builder.Services.AddScoped(typeof(IRelationshipService), typeof(RelationshipService));


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IPostService), typeof(PostService));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? string.Empty)),
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
    options.ClientId = builder.Configuration["GoogleAuthSetting:ClientID"];
    options.ClientSecret = builder.Configuration["GoogleAuthSetting:ClientSecret"];
    options.CallbackPath = "/signin-google";
});

builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddSignalR();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

//seed data
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<UserEntity>>();

    var dbContext = services.GetRequiredService<SocialNetworkdDataContext>();

    try
    {
        await dbContext.Database.MigrateAsync();

        await SeedData.Initialize(services, userManager);
    }catch(Exception e)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while seeding the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialNetwork.Web V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<JWTCookieAuthenticateMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatPerson");
app.MapHub<PostHub>("/Post");
app.Run();

