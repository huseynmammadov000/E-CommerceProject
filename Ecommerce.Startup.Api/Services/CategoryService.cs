using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Models;
using Ecommerce.Startup.Api.Repositories.Abstractions;
using Ecommerce.Startup.Api.Requests;
using Ecommerce.Startup.Api.Services.Abstractions;
using Mapster;

namespace Ecommerce.Startup.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> AddAsync(CreateCategoryRequest entity)
        {
            var category = new Category() { Id = Guid.NewGuid(),Name = entity.Name,CreatedTime = DateTime.UtcNow };

            return await _categoryRepository.AddAsync(category.Adapt<CategoryDto>());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();

        }

        public async Task<CategoryDto> GetByCategoryNameAsync(string categoryName)
        {
            return await _categoryRepository.GetByCategoryNameAsync(categoryName);
        }

        public Task<CategoryDto> GetByIdAsync(Guid id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(UpdateCategoryRequest entity)
        {
            var category = new CategoryDto()
            {
                Name = entity.Name,
            };
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
