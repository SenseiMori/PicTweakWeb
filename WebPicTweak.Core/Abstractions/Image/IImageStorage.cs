using Microsoft.AspNetCore.Http;

namespace WebPicTweak.Core.Abstractions.Image
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(IFormFile file, string DirectoryName);
        string GetStoragePath(Guid userId);
    }
}
