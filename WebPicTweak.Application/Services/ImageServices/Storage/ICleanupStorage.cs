
namespace WebPicTweak.Application.Services.ImageServices.Storage
{
    public interface ICleanupStorage
    {
        Task DeleteOldFolders(CancellationToken ct);
    }
}