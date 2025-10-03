using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Const;

namespace WebPicTweak.Application.Services.ImageServices.Resize
{
    public class ImageResize: IImageHandlerAsync
    {
        private readonly SizeScale _scale;
        private readonly Scaler _scaler = new Scaler();

        public ImageResize(SizeScale scale)
        {
            _scale = scale;
        }
        public async Task<byte[]> Handler(string path)
        {
            using (Image image = await Image.LoadAsync(path))
            {
                (int, int) widthAndHeight = _scaler.GetScaledSize(image.Width, image.Height, _scale);
                using (MemoryStream imageStream = new MemoryStream())
                {
                    image.Mutate(x => x.Resize(widthAndHeight.Item1, widthAndHeight.Item2));
                    await image.SaveAsync(imageStream, new JpegEncoder());
                    return imageStream.ToArray();
                }
            }
        }
        public (int, int) GetJPGSize(Stream MyImage)
        {
            ImageInfo info = Image.Identify(MyImage);
            return (info.Width, info.Height);
        }
    }
}
