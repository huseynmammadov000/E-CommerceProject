using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Requests;

namespace Ecommerce.Startup.Api.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(Guid id);

        Task<CategoryDto> GetByCategoryNameAsync(string categoryName);

        Task<List<CategoryDto>> GetAllAsync();

        Task<CategoryDto> AddAsync(CreateCategoryRequest entity);

        Task UpdateAsync(UpdateCategoryRequest entity);

        Task DeleteAsync(Guid id);
    }
}
