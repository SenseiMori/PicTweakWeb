using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPicTweak.Core.Models.SeedWork;

namespace WebPicTweak.Core.Models.Image
{
    public class ImageId : TypedIdValueBase
    {
        public ImageId (Guid id) : base(id)
        {

        }
        public static ImageId New() => new ImageId(Guid.NewGuid());
    }
}
