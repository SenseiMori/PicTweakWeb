using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebPicTweak.API.DTOs;
using WebPicTweak.API.DTOs.Const;
using WebPicTweak.Application.Services.ImageServices.RemoveExif.JPG;
using WebPicTweak.Core.Abstractions.Image;


namespace WebPicTweak.API.Controllers.Image
{
    [Route("/api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<ImagesController> _logger;
        private readonly IJpegInfo _jpegInfo;
        private readonly JPGFile _jpgFile;

        public ImagesController(IImageStorage imageStorage, ILogger<ImagesController> logger, IJpegInfo jpegInfo, JPGFile jPGFile)
        {
            _imageStorage = imageStorage;
            _logger = logger;
            _jpegInfo = jpegInfo;
            _jpgFile = jPGFile;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            Random rnd = new Random();

            int num = rnd.Next(1, 100);

            return "Image service is running" + num;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<JpegResponse>> UploadImage([FromForm] JpegRequest image)
        {
            //validation: тип файла, вес (не больше 5мб пр.), исполняемость файла,
            try
            {
                if (image.JpegFile.Length > JpegConsts.MaxSize)
                {
                    return BadRequest("Слишком большой файл");
                }

                string pathToSavedJpeg = await _imageStorage.SaveAsync(image.JpegFile);
                var jpeg = await _jpegInfo.GetInfo(pathToSavedJpeg);

                var imageDTO = new JpegResponse
                {
                    FileName = jpeg.FileName,
                    Width = jpeg.Width,
                    Height = jpeg.Height,
                };
                _logger.LogInformation("Image uploaded successfully. FileName: {FileName}, Width: {Width}, Height: {Height}", imageDTO.FileName, imageDTO.Width, imageDTO.Height);
                return CreatedAtAction(nameof(UploadImage), pathToSavedJpeg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<JpegResponse>> ReadEXIFFromImage(string file)
        {
            var markers = await _jpgFile.GetMarkersAppSegment(file);

            foreach (var marker in markers)
            {
                _logger.LogInformation("Marker: {Marker}", marker);
                
            }
            //_logger.LogInformation("Image uploaded successfully. FileName: {FileName}, Width: {Width}, Height: {Height}", imageDTO.FileName, imageDTO.Width, imageDTO.Height);

            return Ok();    


        }
    }
}