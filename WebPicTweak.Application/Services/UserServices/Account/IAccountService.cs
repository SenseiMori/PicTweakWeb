using WebPicTweak.Core.Models.Users;
namespace WebPicTweak.Application.Services.UserServices.Registration
{
    public interface IAccountService
    {
        Task<Account> RegisterAccountAsync(string nickName, string email, string password, CancellationToken ct);
        Task<GuestAccount> CreateGuestAccountAsync(CancellationToken ct);
        Task<BaseAccount> GetAccountByIdAsync(Guid id);
        Task<GuestAccount> GetGuestAccountByIdAsync(Guid id, CancellationToken ct);
        Task<string> LoginAsync(string email, string password);
    }
}