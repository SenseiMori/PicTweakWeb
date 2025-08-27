using WebPicTweak.Application.Services.ImageServices.Const;

namespace WebPicTweak.Application.Services.ImageServices.Resize
{
    public class Scaler
    {
        public (int, int) GetScaledSize(int width, int height, SizeScale sizeScale)
        {
            double scale = (int)sizeScale / 100.0;
            int newWidth = (int)(width * scale);
            int newHeight = (int)(height * scale);
            return (newWidth, newHeight);
        }
        public string ConvertToNewSize((int, int) oldJPGSize, SizeScale scale)
        {
            (int, int) expectedSize = GetScaledSize(oldJPGSize.Item1, oldJPGSize.Item2, scale);
            string result = $"{expectedSize.Item1}x{expectedSize.Item2}";
            return result;
        }
    }
}
