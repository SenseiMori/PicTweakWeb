using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPicTweak.Core.Models.SeedWork;

namespace WebPicTweak.Core.Models.Users
{
    public class UserId :TypedIdValueBase
    {
        public UserId(Guid id) : base(id)
        {

        }
    }
}
