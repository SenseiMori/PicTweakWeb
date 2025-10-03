using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Application.Services.UserServices
{
    public interface IJwtProvider
    {
        string GenerateToken(Account user);
    }
}