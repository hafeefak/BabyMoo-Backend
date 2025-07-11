using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Category;
using BabyMoo.Models;
using BabyMoo.Services.Category;
using Microsoft.AspNetCore.Authorization;

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

        // ✅ Get all categories
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryViewDto>>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(new ApiResponse<List<CategoryViewDto>>(200, "Categories retrieved", categories));
        }

        // ✅ Add a new category (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryViewDto categoryDto)
        {
            await _categoryService.AddCategory(categoryDto);
            return Ok(new ApiResponse<string>(200, "Category added successfully"));
        }

        // ✅ Delete category by name (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryName}")]
        public async Task<IActionResult> DeleteCategoryByName(string categoryName)
        {
            await _categoryService.DeleteCategoryByName(categoryName);
            return Ok(new ApiResponse<string>(200, $"Category '{categoryName}' deleted successfully"));
        }
    }
}
