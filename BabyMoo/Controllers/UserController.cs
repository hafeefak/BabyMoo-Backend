using BabyMoo.DTOs.User;
using BabyMoo.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyMoo.Models; 
using BabyMoo.Middleware; 

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
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
            return Ok(new ApiResponse<List<UserViewDto>>(200, "User list retrieved", users));
        }

       
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                throw new NotFoundException("User not found");

            return Ok(new ApiResponse<UserViewDto>(200, "User retrieved", user));
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("users/block/{id}")]
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var result = await _userService.ToggleBlock(id);
            if (!result)
                throw new NotFoundException("User not found");

            return Ok(new ApiResponse<string>(200, "User block status updated"));
        }
    }
}
