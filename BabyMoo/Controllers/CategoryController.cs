using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Category;
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
        public async Task<ActionResult<List<CategoryViewDto>>> GetAll()
        {
            return await _categoryService.GetAllCategories();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewDto categoryDto)
        {
            var result = await _categoryService.AddCategory(categoryDto);
            if (result)
                return Ok("Category added successfully.");
            return BadRequest("Failed to add category.");
        }
    }

}
