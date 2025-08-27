using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Core.Abstractions.Image
{
    public interface IJpegInfo
    {
        Task<Jpeg> GetInfo(string path);
    }
}