

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Services.ImageServices.Storage;
using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Infrastructure.Services.BackgroundServices
{
    public class CleanupBackgroundService : ICleanupBackgroundService
    {
        private readonly Channel<FolderJobDTO> _folderJob;

        public CleanupBackgroundService()
        {
            _folderJob = Channel.CreateUnbounded<FolderJobDTO>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }
        public async ValueTask EnqueueAsync(FolderJobDTO job, CancellationToken ct)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            await _folderJob.Writer.WriteAsync(job, ct);
        }
        public async ValueTask<FolderJobDTO> DequeueAsync(CancellationToken ct)
        {
            var job = await _folderJob.Reader.ReadAsync(ct);
            return job;
        }
    }
    public class BackgroundCleanup : BackgroundService
    {
        private readonly ICleanupStorage _cleanupStorage;
        private readonly ILogger<BackgroundCleanup> _logger;    

        private readonly TimeSpan _interval = TimeSpan.FromMinutes(2); 
        public BackgroundCleanup(ILogger<BackgroundCleanup> logger, ICleanupStorage cleanupStorage)
        {
            _cleanupStorage = cleanupStorage;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Чистка папок");
                    await _cleanupStorage.DeleteOldFolders(ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при очистке папок");
                }
                await Task.Delay(_interval, ct);
            }
        }
    }
}

