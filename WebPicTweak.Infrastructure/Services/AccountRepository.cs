using Microsoft.EntityFrameworkCore;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Infrastructure.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LogDbContext _context;
        public AccountRepository(LogDbContext context)
        {
            _context = context;
        } 
        public async Task SaveAsync(BaseAccount account)
        {
            var addedAcc = await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        public async Task<Account> GetByEmailAsync(string email)
        {
            var userEntity = await _context.Accounts.OfType<Account>().AsNoTracking().FirstOrDefaultAsync(x => x.Email.Value == email);
            if (userEntity is null)
            {
                return null;
            }
            return userEntity;
        }
        public async Task<BaseAccount?> GetAccountByIdAsync(Guid? accountId)
        {
            var userEntity = await _context.Accounts.Include(x => x.UserLog)
                .FirstOrDefaultAsync(a => a.Id == accountId);
            if (userEntity is null)
            {
                return null;
            }
            return userEntity;
        }
        public async Task<GuestAccount?> GetGuestAccountByIdAsync(Guid? accountId)
        {
            var userEntity = await _context.Accounts.OfType<GuestAccount>()
                .FirstOrDefaultAsync(a => a.Id == accountId);
            if (userEntity is null)
            {
                return null;
            }
            return userEntity;
        }
    }
}
