using Microsoft.AspNetCore.SignalR;

namespace WebPicTweak.API.Notify
{
    public class ArchiveHub : Hub
    {
        private readonly ILogger<ArchiveHub> _logger;
        public ArchiveHub(ILogger<ArchiveHub> logger) { _logger = logger; }
        public Task Register(string sessionId)
        {
            _logger.LogInformation("Register called. ConnectionId={conn}, userId={uid}", Context.ConnectionId, sessionId);
            return Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }
    }
}
