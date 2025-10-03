using WebPicTweak.Application.Services.ImageServices.Compress;
using WebPicTweak.Application.Services.ImageServices.RemoveExif.JPG;
using WebPicTweak.Application.Services.ImageServices.Resize;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Const;

namespace WebPicTweak.Application.Services.ImageServices.ImageHandlersContext
{
    public class ImageHandlerFactory : IImageHandlerFactory
    {
        private readonly IServiceProvider _provider;
        public ImageHandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public IImageHandlerAsync CreateCompress(CompressLevel level)
        {
            return new CompressHandler(level);
        }
        public IImageHandlerAsync CreateRemoveExif()
        {
            return new JPGFile();
        }
        public IImageHandlerAsync CreateResize(SizeScale scale)
        {
            return new ImageResize(scale);   
        }
    }
}
