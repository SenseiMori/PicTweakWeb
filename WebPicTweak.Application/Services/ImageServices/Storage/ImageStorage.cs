using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebPicTweak.Application.Options;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Application.Services.ImageServices.Storage
{
    public class ImageStorage : IImageStorage
    {
        private readonly string _basePath;

        public ImageStorage(IOptions<StorageOptions> options)
        {
            _basePath = options.Value.ImagesStorage;
        }
        public string GetStoragePath(Guid userId)
        {
            string UserStorage = Path.Combine(_basePath, userId.ToString());
            return UserStorage;
        }
        public async Task<string> SaveAsync(IFormFile file, string DirectoryName)
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            string fullPath = Path.Combine(DirectoryName, file.FileName);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(fs);
                return fullPath;
            }
        }  
    }
}
