using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebPicTweak.Infrastructure.Services.JWT;

namespace WebPicTweak.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAutentication(this IServiceCollection services, IOptions<JwtOptions> options)
        {
            //var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = options.Value.SecretKey;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tres"];
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
                        logger.LogError("Authentication failed.", context.Exception);
                        return Task.CompletedTask;
                    },
                };
            });
            services.AddAuthorization();
        }

        public static Guid FindUserById(ClaimsPrincipal user)
        {
            string? se = user.FindFirst("sub")?.Value ??
                                        user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                        user.FindFirst("id")?.Value;

            if (Guid.TryParse(se, out Guid userId))
                return userId;
            else
                return Guid.Empty;
        }

        public static string? GetUserEmail(ClaimsPrincipal user)
        {
            return user.FindFirst("email")?.Value ??
                                       user.FindFirst(ClaimTypes.Email)?.Value ??
                                       user.FindFirst("preferred_username")?.Value ??
                                       user.Identity.Name;
        }

    }
}
