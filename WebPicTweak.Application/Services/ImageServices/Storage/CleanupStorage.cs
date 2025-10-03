using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using WebPicTweak.Application.Options;

namespace WebPicTweak.Application.Services.ImageServices.Storage
{
    public class CleanupStorage : ICleanupStorage
    {
        private readonly string _basePath;
        private readonly TimeSpan _folderLifetime = TimeSpan.FromMinutes(5);

        public CleanupStorage(IOptions<StorageOptions> options)
        {
            _basePath = options.Value.ImagesStorage;
        }
        public Task DeleteOldFolders(CancellationToken ct)
        {
            var storage = Directory.GetDirectories(_basePath);

            foreach (var folder in storage)
            {
                var directoryInfo = new DirectoryInfo(folder);

                if (DateTime.Now - directoryInfo.CreationTime > _folderLifetime)
                {
                    directoryInfo.Delete(true);
                }
            }
            return Task.CompletedTask;
        }
    }
}
