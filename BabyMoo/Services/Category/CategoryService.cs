using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Category;
using BabyMoo.Models;
using BabyMoo.Middleware;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryViewDto>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryViewDto>>(categories);
        }

 
        public async Task<bool> AddCategory(CategoryViewDto categoryDto)
        {
            if (categoryDto == null)
                throw new BadRequestException("Category data is required");

            bool exists = await _context.Categories
                .AnyAsync(c => c.CategoryName.ToLower() == categoryDto.CategoryName.ToLower());

            if (exists)
                throw new ConflictException("Category already exists");

            var category = _mapper.Map<Models.Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
