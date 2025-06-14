using BabyMoo.DTOs.Category;

namespace BabyMoo.Services.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryViewDto>> GetAllCategories();
        Task<bool> AddCategory(CategoryViewDto categoryDto);
    }

}
