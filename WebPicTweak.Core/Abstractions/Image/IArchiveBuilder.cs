namespace WebPicTweak.Core.Abstractions.Image
{
    public interface IArchiveBuilder
    {
        Task<string> CreateArchiveAsync(string[] pathToImages, string DirectoryName);
    }
}