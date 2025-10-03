
namespace WebPicTweak.Core.Abstractions.Image
{
    public interface IImageHandler
    {
        byte[] Handler(byte[] data);
    }

    public interface IImageHandlerAsync
    {
        Task<byte[]> Handler(string path);
    }
}