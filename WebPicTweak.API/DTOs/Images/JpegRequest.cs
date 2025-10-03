using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.API.DTOs.Images
{
    public class JpegRequest
    {
        public List <IFormFile> Files { get; set; }
        public ModifierOptions Options { get; set; }
        public List <int> Width { get; set; } = new List<int>();
        public List<int> Height { get; set; } = new List<int>();


    }
}
