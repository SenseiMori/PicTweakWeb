using Microsoft.AspNetCore.Http;
using System.Drawing;
using WebPicTweak.Core.Models.SeedWork;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Core.Models.Image
{
    public class Jpeg
    {
        public ImageId Id { get; set; }
        public UserId UserId { get; set; }
        public List<byte> JPGMarkers { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ShortFileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string WidthHeight { get; set; }
        public string Size { get; set; }
        public string ExpectedWidthHeight { get; set; }

    }
    // public string WidthHeight { get; } = string.Empty;
    // public string ExpectedWidthHeight { get; } = string.Empty;
    // public string ExpectedSize { get; } = string.Empty;

}