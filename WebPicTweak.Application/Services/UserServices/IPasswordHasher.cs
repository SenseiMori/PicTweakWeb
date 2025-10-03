namespace WebPicTweak.Application.Services.UserServices
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string hashedPassword, string providedPassword);
    }
}