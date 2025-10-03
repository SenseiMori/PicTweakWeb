namespace WebPicTweak.API.DTOs.Images
{
    public class JpegResponse
    {
        public IFormFile JpegFile { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string ExpectedWidthHeight { get; set; }
        public string Size { get; set; }

    }
}
