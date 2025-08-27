using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using WebPicTweak.Application.Services.ImageServices.Const;

namespace WebPicTweak.Application.Services.ImageServices.Resize
{
    public class ImageResize
    {
        Scaler scaler = new Scaler();
        public async Task<byte[]> ResizeJPG(string path, SizeScale scale)
        {
            using (Image image = await Image.LoadAsync(path))
            {
                (int, int) widthAndHeight = scaler.GetScaledSize(image.Width, image.Height, scale);
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
