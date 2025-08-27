using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Core.Models.Users
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(ImageId id);
        Task AddAsync(User user);
    }
}
