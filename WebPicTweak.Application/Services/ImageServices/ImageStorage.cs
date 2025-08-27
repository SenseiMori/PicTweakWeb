using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebPicTweak.Application.Options;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Application.Services.ImageServices
{
    public class ImageStorage: IImageStorage
    {
        private readonly string _basePath;
        public ImageStorage(IOptions<StorageOptions> options)
        {
            _basePath = options.Value.ImagesStorage;

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory("/localdata/images");
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine("/localdata/images", fileName); 
            using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                
                await file.CopyToAsync(fs);
                return fullPath;
            }
        }
        public Task<bool> DeleteAsync(string filePath)
        {
            string path = Path.Combine(_basePath, filePath);
            if (!File.Exists(path))
            {
                return Task.FromResult(false);
            }
            File.Delete(path);
            return Task.FromResult(true);
        }

        public Task<Jpeg?> GetByIdAsync(ImageId id)
        {
            throw new NotImplementedException();
        }


    }
}
