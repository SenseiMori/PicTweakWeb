using Microsoft.EntityFrameworkCore;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Infrastructure.Services
{
    public  class AdminRepository : IAdminRepository
    {
        private readonly LogDbContext _context;
        public AdminRepository(LogDbContext context)
        {
            _context = context;
        }

        public async Task<List<SessionLog>> GetAllSessionsAtLastDay()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);

            var userEntity = await _context.SessionLogs
                .Where(x => x.SessionCreatedAt == yesterday)
                .ToListAsync();

            return userEntity;
        }
        public async Task<List<BaseAccount>> GetAllRegistrationAtLastDay()
        {
            var registrationsAtLastDay = await _context.Accounts
                .Where(u => u.RegistrationDate >= DateTime.UtcNow.AddDays(-1))
                .ToListAsync();
            return registrationsAtLastDay;
        }
    }
}
