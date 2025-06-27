using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Auth;
using BabyMoo.Service.AuthService;
using BabyMoo.Models;
using BabyMoo.Middleware; 

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
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid registration data.");

            await _authService.RegisterAsync(regDto); 
            return Ok(new ApiResponse<string>(200, "Registration successful"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginDto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid login data.");

            var result = await _authService.LoginAsync(loginDto); 
            return Ok(new ApiResponse<ResultDto>(200, "Login successful", result));
        }
    }
}
