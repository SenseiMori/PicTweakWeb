using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Core.Models.Users
{
    public interface IAccountRepository
    {
        Task SaveAsync(BaseAccount account);
        Task<BaseAccount?> GetAccountByIdAsync(Guid? accountId);
        Task<Account> GetByEmailAsync(string email);
        Task<GuestAccount?> GetGuestAccountByIdAsync(Guid? accountId);
    }
}
