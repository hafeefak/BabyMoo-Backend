using BabyMoo.DTOs.User;
using BabyMoo.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    public class   UserController: ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPut("users/block/{id}")]
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var result = await _userService.ToggleBlock(id);
            return result ? Ok(new { message = "User block status updated." }) : NotFound();
        }

        
    }
}
