using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebPicTweak.API.DTOs.Images;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Services.ImageServices.ModifierProcess;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Abstractions.Log;
using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.API.Controllers.Image
{
    [Route("/api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IImageModifierProcess _imageModifierProcess;
        private readonly ISessionStore _sessionStore;

        public ImagesController(ILogger<ImagesController> logger,
                                IImageProcessingBackgroundService backgroundTaskQueue,
                                IArchiveBuilder archiveBuilder,
                                ISessionStore sessionStore,
                                ILog sessionLog,
                                IImageModifierProcess imageModifierProcess,
                                IImageStorage imageStorage)
        {
            _logger = logger;
            _sessionStore = sessionStore;
            _imageModifierProcess = imageModifierProcess;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<JpegResponse>> UploadImage([FromForm] JpegRequest JpegFiles, CancellationToken ct)
        {
            var User = Request.Form["user"].FirstOrDefault();

            if (string.IsNullOrEmpty(User) || !Guid.TryParse(User, out var UserGuid))
            {
                return BadRequest($"Пользователь:{User} не найден");
            }
            try
            {
                Guid UserId = Guid.Parse(User);
                List<JpegData> files = JpegFiles.Files.Select((file, index) => new JpegData
                {
                    FileData = file,
                    FileName = file.FileName,
                    Height = JpegFiles.Height[index],
                    Width = JpegFiles.Width[index],
                    Size = file.Length,
                }).ToList();

                await _imageModifierProcess.ModifierProcessAsync(UserId, files, JpegFiles.Options, ct);
                return Ok(new { success = true });
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogInformation(ex, "Image processing task cancelled by client.");
                return StatusCode(499, new { error = "Request cancelled by client." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during image upload: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An internal server error occurred." });
            }
        }




        [HttpGet("{sessionId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult DownloadArchive(Guid sessionId)
        {
            try
            {
                
                var session = _sessionStore.GetSession(sessionId);
                if (session == null)
                {
                    _logger.LogWarning("Ошибка скачивания: сессия {SessionId} не найдена", sessionId);
                    return NotFound(new { error = "Архив не найден." });
                }
                const string contentType = "application/zip";

                return PhysicalFile(session.PathToZip, contentType);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка скачивания: сессия {SessionId} не найдена", sessionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "При скачивании что-то пошло не так." });
            }
        }

    }
}