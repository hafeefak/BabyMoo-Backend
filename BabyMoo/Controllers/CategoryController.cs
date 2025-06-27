using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Category;
using BabyMoo.Models;
using BabyMoo.Services.Category;

namespace BabyMoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryViewDto>>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(new ApiResponse<List<CategoryViewDto>>(200, "Categories retrieved", categories));
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryViewDto categoryDto)
        {
            await _categoryService.AddCategory(categoryDto);
            return Ok(new ApiResponse<string>(200, "Category added successfully"));
        }
    }
}
