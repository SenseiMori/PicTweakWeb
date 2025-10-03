using Microsoft.AspNetCore.SignalR;
using WebPicTweak.Application.Abstractions;

namespace WebPicTweak.API.Notify
{
    public class ProgressNotifier : IProgressNotifier
    {
        private readonly IHubContext<ArchiveHub> _hubContext;
        private readonly ILogger<ProgressNotifier> _logger;

        public ProgressNotifier(
                                IHubContext<ArchiveHub> hubContext,
                                ILogger<ProgressNotifier> logger
                                )
        {
            _hubContext = hubContext;
            _logger = logger;
        }
        public async Task NotifyProgressAsync(Guid userId, string fileName, int percent, string status, CancellationToken ct)
        {
            try
            {
                await _hubContext.Clients.Group(userId.ToString())
                    .SendAsync("ProgressUpdate", new
                    {
                        userId = userId.ToString(),
                        fileName = fileName,
                        percent = percent,
                        status = status
                    }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении прогресса");
            }
        }
        public async Task NotifyArchiveReadyAsync(Guid userId, Guid sessionId, DateTime expiresAt, CancellationToken ct)
        {
            try
            {
                await _hubContext.Clients.Group(userId.ToString())
                    .SendAsync("ArchiveReady", new
                    {
                        userId = userId,
                        sessionId = sessionId,
                        expiresAt = expiresAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                    }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании архива");
            }
        }
    }
}

