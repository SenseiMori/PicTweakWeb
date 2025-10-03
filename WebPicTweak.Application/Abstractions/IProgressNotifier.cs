namespace WebPicTweak.Application.Abstractions
{
    public interface IProgressNotifier
    {
        Task NotifyProgressAsync(Guid userId, string fileName, int percent, string status, CancellationToken ct);
        Task NotifyArchiveReadyAsync(Guid userId, Guid sessionId, DateTime expiresAt, CancellationToken ct);
    }
}
