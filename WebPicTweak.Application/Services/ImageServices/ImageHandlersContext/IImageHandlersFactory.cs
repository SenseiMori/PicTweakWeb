using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Const;

namespace WebPicTweak.Application.Services.ImageServices.ImageHandlersContext
{
    public interface IImageHandlerFactory
    {
        IImageHandlerAsync CreateResize(SizeScale scale);
        IImageHandlerAsync CreateCompress(CompressLevel level);
        IImageHandlerAsync CreateRemoveExif();
    }
}
