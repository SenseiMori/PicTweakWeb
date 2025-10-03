using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Const;

namespace WebPicTweak.Application.Services.ImageServices.Compress
{
    public class CompressHandler : IImageHandlerAsync
    {
        ImageCompressor _compressor = new();
        private readonly CompressLevel _compressLevel;

        public CompressHandler(CompressLevel compressLevel)
        {
            _compressLevel = compressLevel;
        }
        public async Task<byte[]> Handler(string path)
        {
            byte[] data = Array.Empty<byte>();
            data = await _compressor.JPGCompress(path, _compressLevel);
            return data;
        }
    }
}