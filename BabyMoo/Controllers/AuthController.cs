using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Auth;
using BabyMoo.Service.AuthService;

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register regDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _authService.RegisterAsync(regDto);

                if (!success)
                {
                    return Conflict(new { message = "User already exists." });
                }

                return Ok(new { message = "Registration successful." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);

                if (!string.IsNullOrEmpty(result.Error))
                {
                    return Unauthorized(new { message = result.Error });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
