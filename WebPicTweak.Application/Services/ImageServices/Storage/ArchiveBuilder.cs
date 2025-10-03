using System.IO.Compression;
using WebPicTweak.Core.Abstractions.Image;

namespace WebPicTweak.Application.Services.ImageServices.Storage
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public async Task<string> CreateArchiveAsync(string[] pathToImages, string DirectoryName)
        {
            {
                string ArchiveName = "FromPicTweakWithLove.zip";
                string ArchivePath = Path.Combine(DirectoryName, ArchiveName);

                using var outStream = new FileStream(ArchivePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, true);
                using var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
                foreach (string fileName in pathToImages)
                {
                    using (var inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                    {
                        ZipArchiveEntry fileEntry = archive.CreateEntry(Path.GetFileName(fileName));
                        using var entryStream = fileEntry.Open();
                        await inputStream.CopyToAsync(entryStream);
                    }
                }
                return ArchivePath;
            }
        }
    }
}
