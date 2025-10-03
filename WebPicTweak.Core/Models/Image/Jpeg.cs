using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Core.Models.Image
{
    public class Jpeg
    {
        public List<byte> JPGMarkers { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string WidthHeight { get; set; }
        public string Size { get; set; }
        public string ExpectedWidthHeight { get; set; }

    }
}