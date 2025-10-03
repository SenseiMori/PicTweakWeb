using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Application.Services.ImageServices.ModifierProcess
{
    public interface IImageModifierProcess
    {
        Task ModifierProcessAsync(Guid userId, IReadOnlyList<JpegData> files, ModifierOptions options, CancellationToken ct);
    }
}