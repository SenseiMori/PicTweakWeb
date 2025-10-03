using Microsoft.AspNetCore.Mvc;
using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.API.Controllers.Users
{
    [Route("api/admin")]
    public class AdminPanelController : Controller
    {
        private readonly ILogRepository _logRepo;

        public AdminPanelController(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }
        [HttpGet("logs")]
        public async Task<IActionResult> GetData()
        {
            var logs = await _logRepo.GetAllSessionsAtLastDay();

            return Ok(logs);
        }
    }
}
