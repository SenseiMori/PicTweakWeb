using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Services.ImageServices.ImageHandlersContext;
using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Infrastructure.Services.BackgroundServices
{
    public class ImageProcessingBackground : IImageProcessingBackgroundService
    {
        private readonly Channel<JpegJobDTO> _queue;

        public ImageProcessingBackground()
        {
            _queue = Channel.CreateUnbounded<JpegJobDTO>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        public async ValueTask EnqueueAsync(JpegJobDTO job, CancellationToken ct)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            await _queue.Writer.WriteAsync(job, ct);
        }

        public async ValueTask<JpegJobDTO> DequeueAsync(CancellationToken ct)
        {
            var job = await _queue.Reader.ReadAsync(ct);
            return job;
        }
    }
    public class ImageProcessingBackgroundService : BackgroundService
    {
        private readonly IImageProcessingBackgroundService _taskQueue;
        private readonly ILogger<ImageProcessingBackgroundService> _logger;
        private readonly IServiceProvider _services;
        private readonly SemaphoreSlim _concurrency = new SemaphoreSlim(4);
        public ImageProcessingBackgroundService(
            IImageProcessingBackgroundService taskQueue,
            IServiceProvider services,
            ILogger<ImageProcessingBackgroundService> logger)
        {
            _taskQueue = taskQueue;
            _services = services;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var job = await _taskQueue.DequeueAsync(ct);

                await _concurrency.WaitAsync(ct);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _services.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<IMainHandler>();

                        var cmd = new JpegJobDTO
                        {
                            FilePath = job.FilePath,
                            Options = job.Options,
                            TransactionId = job.TransactionId

                        };
                        //нужен токен отмены
                        await handler.Processing(cmd);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Ошибка при обработке: {ex.Message}. TransactionId: {job.TransactionId}");
                    }
                    finally
                    {
                        _concurrency.Release();
                    }
                }, ct);
            }
        }
    }
}
