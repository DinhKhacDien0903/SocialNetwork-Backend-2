using Microsoft.IdentityModel.Tokens;

namespace SocialNetwork.Web.Middlewares
{
    public class JWTCookieAuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        public JWTCookieAuthenticateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            var authorService = serviceProvider.GetRequiredService<IAuthorService>();

            var accessToken = context.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var principal = authorService.ValidateAccessToken(accessToken);

                    if (principal != null)
                    {
                        context.User = principal;
                    }

                }
                catch(SecurityTokenValidationException e)
                {
                    //todo => ghi log
                }

            }
            await _next(context);
        }
    }
}
