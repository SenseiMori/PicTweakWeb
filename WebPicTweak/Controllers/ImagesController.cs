using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PTWeb.Controllers
{
    [Route("/api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetImages()
        {
            var images = new[]
            {
                new { Name = "1" },
                new { Name = "2" },
            };
            return Ok(images);
        }

    }


}