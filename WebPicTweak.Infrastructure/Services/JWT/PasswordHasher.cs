using WebPicTweak.Application.Services.UserServices;

namespace WebPicTweak.Infrastructure.Services.JWT
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        public bool Verify(string hashedPassword, string providedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
    }
}
