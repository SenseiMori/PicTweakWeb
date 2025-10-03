using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Transactions;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Abstractions.Log;
using WebPicTweak.Core.Models.Image;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Application.Services.ImageServices.ModifierProcess
{
    public class ImageModifierProcess : IImageModifierProcess
    {
        private readonly IImageStorage _imageStorage;
        private readonly IImageProcessingBackgroundService _taskQueue;
        private readonly ILog _sessionLog;
        private readonly IProgressNotifier _notifier;
        private readonly IArchiveBuilder _archiveBuilder;
        private readonly ISessionStore _sessionStore;
        private readonly IAccountRepository _userRepository;
        private readonly ILogger<ImageModifierProcess> _logger;

        public ImageModifierProcess(IImageStorage imageStorage,
            IImageProcessingBackgroundService taskQueue,
            ILog log,
            IProgressNotifier notifier,
            IArchiveBuilder archiveBuilder,
            ISessionStore sessionStore,
            IAccountRepository userRepository,
            ILogger<ImageModifierProcess> logger)
        {
            _imageStorage = imageStorage;
            _taskQueue = taskQueue;
            _sessionLog = log;
            _notifier = notifier;
            _archiveBuilder = archiveBuilder;
            _sessionStore = sessionStore;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task ModifierProcessAsync(Guid userId, IReadOnlyList<JpegData> files, ModifierOptions options, CancellationToken ct)
        {
            try
            {
                string pathToImages = _imageStorage.GetStoragePath(Guid.NewGuid());

                var user = await _userRepository.GetAccountByIdAsync(userId);
                var sessionLog = SessionLog.Create(user.UserLog.UserLogId);

                user.UserLog.AddSession(sessionLog);
                await _sessionLog.SaveAsync(sessionLog, ct);
                List<ImageLog> imageLogs = new List<ImageLog>();
                List<string> resultImages = new List<string>();
                Stopwatch sw = new Stopwatch();

                var totalFiles = files.Count;
                var processedFiles = 0;

                foreach (var image in files)
                {
                    sw.Start();
                    string WidthHeight = $"{image.Width}x{image.Height}";


                    //валидация файла

                    string SavedJpeg = await _imageStorage.SaveAsync(image.FileData, pathToImages);
                    resultImages.Add(SavedJpeg);
                    var job = new JpegJobDTO
                    {
                        TransactionId = Guid.NewGuid(),
                        FileName = image.FileName,
                        FilePath = SavedJpeg,
                        Options = options,
                        File = image.FileData
                    };
                    await _taskQueue.EnqueueAsync(job, ct);
                    sw.Stop();
                    var elapsedMs = sw.ElapsedMilliseconds;
                    var progress = (int)((processedFiles / (double)totalFiles) * 100);
                    await _notifier.NotifyProgressAsync(userId, job.FileName, progress, "success", ct);
                    processedFiles++;
                    ImageLog imageLog = ImageLog.Create(sessionLog.SessionId, elapsedMs, WidthHeight, image.Size);
                    imageLogs.Add(imageLog);
                }
                await _sessionLog.LogBatchAsync(imageLogs, ct);
                string path = await _archiveBuilder.CreateArchiveAsync(resultImages.ToArray(), pathToImages);
                _sessionStore.SetPath(sessionLog.SessionId, path);
                await _notifier.NotifyArchiveReadyAsync(userId, sessionLog.SessionId, DateTime.Now, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке изображения: {Message}", ex.Message);

            }
        }
    }
}
