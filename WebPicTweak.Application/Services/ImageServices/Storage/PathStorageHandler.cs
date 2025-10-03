using Microsoft.Extensions.Options;
using WebPicTweak.Application.Options;

namespace WebPicTweak.Application.Services.ImageServices.Storage
{
    public class PathStorageHandler
    {
        private readonly string _basePath;
        public PathStorageHandler(IOptions<StorageOptions> options)
        {
            _basePath = options.Value.ImagesStorage;
        }
        public string GetStoragePath(Guid userId)
        {
            string UserStorage = Path.Combine(_basePath, userId.ToString());
            return UserStorage;
        }


    }
}
