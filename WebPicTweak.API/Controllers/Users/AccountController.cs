using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebPicTweak.API.DTOs.Users;
using WebPicTweak.API.Extensions;
using WebPicTweak.Application.Services.UserServices.Registration;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.API.Controllers.Users
{
    [Route("api/users")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService authService, ILogger<AccountController> logger)
        {
            _accountService = authService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequest, CancellationToken ct)
        {
            try
            {
                var UserGuid = Guid.NewGuid();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var account = await _accountService.RegisterAccountAsync(registerRequest.Nickname,
                                                        registerRequest.Email,
                                                        registerRequest.Password,
                                                        ct);
                var token = await _accountService.LoginAsync(registerRequest.Email,
                                                          registerRequest.Password);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                Response.Cookies.Delete("guestId");
                return Ok(new
                {
                    token = token,
                    id = expClaim?.Value ?? account.Id.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest registerRequest)
        {
            try
            {


                var token = await _accountService.LoginAsync(registerRequest.Email,
                                                          registerRequest.Password);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);


                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");

                if (expClaim is null)
                {
                    return StatusCode(500, "Нет токена");
                }
                Response.Cookies.Delete("guestId");
                return Ok(new
                {
                    token = token,
                    id = expClaim.Value
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            try
            {
                var newGuest = await _accountService.CreateGuestAccountAsync(ct);
                Response.Cookies.Append("guestId", newGuest.Id.ToString(), new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false, // переключить на true в продакшене
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(10)
                });

                Response.Cookies.Delete("guestId");
                return Ok(new
                {
                    guestId = newGuest.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("isAuthorize")]
        public async Task<IActionResult> CheckAuthorize(CancellationToken ct)
        {
            try
            {
                var user = HttpContext.User;
                if (user?.Identity?.IsAuthenticated == true)
                {

                    Guid userId = ApiExtensions.FindUserById(user);
                    var account = await _accountService.GetAccountByIdAsync(userId);

                    if (account == null)
                        return NotFound(new { error = "Аккаунт не найден" });

                    return Ok(new
                    {
                        isAuthorized = true,
                        id = account.Id.ToString()
                    });
                }
                var getGuestIdFromCookie = Request.Cookies["guestId"];
                if (!string.IsNullOrEmpty(getGuestIdFromCookie) && Guid.TryParse(getGuestIdFromCookie, out Guid existGuestId))
                {
                    var guest = await _accountService.GetGuestAccountByIdAsync(existGuestId, ct);
                    if (guest is GuestAccount)
                    {
                        return Ok(new
                        {
                            isAuthorized = false,
                            guestId = guest.Id.ToString()
                        });
                    }
                }
                var newGuest = await _accountService.CreateGuestAccountAsync(ct);

                Response.Cookies.Append("guestId", newGuest.Id.ToString(), new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false, // В продакшене нужно true
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(10)
                });

                return Ok(new
                {
                    isAuthorized = false,
                    guestId = newGuest.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Что-то пошло не так.");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}

