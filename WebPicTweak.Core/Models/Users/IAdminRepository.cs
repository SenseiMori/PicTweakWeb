using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Core.Models.Users
{
    public interface IAdminRepository
    {
        Task<List<BaseAccount>> GetAllRegistrationAtLastDay();
        Task<List<SessionLog>> GetAllSessionsAtLastDay();
    }
}
