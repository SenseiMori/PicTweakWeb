using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Application.Abstractions
{
    public interface IImageProcessingBackgroundService
    {
        ValueTask EnqueueAsync(JpegJobDTO job, CancellationToken ct);
        ValueTask<JpegJobDTO> DequeueAsync(CancellationToken ct);
    }
}
