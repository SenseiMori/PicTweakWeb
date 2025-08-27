using SixLabors.ImageSharp;
using WebPicTweak.Core.Models.Image;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Application.Services.ImageServices.Compress;
using WebPicTweak.Application.Services.ImageServices.RemoveExif.JPG;
using WebPicTweak.Application.Services.ImageServices.Resize;
//using static System.Net.Mime.MediaTypeNames;

namespace WebPicTweak.Application.Services.ImageServices.Info
{
    public class JpegInfo : IJpegInfo
    {
        List<byte> exifData = new();
        JPGMetadataReader metadataReader = new();
        Scaler scaler = new Scaler();
        ImageResize imageResize = new ImageResize();
        ImageCompressor imageCompressor = new ImageCompressor();

        public async Task<Jpeg> GetInfo(string path)
        {
            exifData = await metadataReader.ReadExifFromImage(path);
            var fileInfo = new FileInfo(path);
            var imageInfo = await Image.IdentifyAsync(path);
            return new Jpeg
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetFullPath(path),
                //ShortFileName = Path.GetFileNameWithoutExtension(path),
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                WidthHeight = $"{imageInfo.Width}x{imageInfo.Height}",
                Size = imageCompressor.GetBytesReadable(fileInfo.Length),
                ExpectedWidthHeight = string.Empty,
                JPGMarkers = exifData
            };
        }
    }
}
