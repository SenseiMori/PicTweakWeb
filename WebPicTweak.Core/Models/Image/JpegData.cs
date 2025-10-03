using Microsoft.AspNetCore.Http;

namespace WebPicTweak.Core.Models.Image
{
    public class JpegData
    {
        public IFormFile FileData { get; set; }
        public string FileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string WidthHeight { get; set; }
        public long Size { get; set; }
    }
}
