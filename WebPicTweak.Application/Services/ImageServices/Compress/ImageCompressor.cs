using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using WebPicTweak.Application.Services.ImageServices.Const;

namespace WebPicTweak.Application.Services.ImageServices.Compress
{
    public class ImageCompressor
    {
        public async Task<byte[]> JPGCompress(string path, CompressLevel compressLevel)
        {
            using (Image image = await Image.LoadAsync(path))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await image.SaveAsync(ms, GetCompressLevel(compressLevel));
                    return ms.ToArray();
                }
            }
        }

        public JpegEncoder GetCompressLevel(CompressLevel level)
        {
            var encoder = new JpegEncoder
            {
                Quality = (int)level,
                SkipMetadata = true,
            };

            return encoder;
        }
        public string GetBytesReadable(long num)
        {
            long absolute_num = (num < 0 ? -num : num);
            string suffix;
            double readable;
            if (absolute_num >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (num >> 10);
            }
            else if (absolute_num >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = num;
            }
            else
            {
                return num.ToString("0 B"); // Byte
            }
            readable = (readable / 1024);
            return readable.ToString("0.#") + suffix;
        }
    }
}
