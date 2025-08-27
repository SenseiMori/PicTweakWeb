using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPicTweak.Core.Models.Image
{
    public interface IImageRepository
    {
        Task <Jpeg> GetByIdAsync (ImageId id);


    }
}
