using Ecommerce.Startup.Api.DTOs;

namespace Ecommerce.Startup.Api.Repositories.Abstractions
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> GetByIdAsync(Guid id);

        Task<CategoryDto> GetByCategoryNameAsync(string categoryName);

        Task<List<CategoryDto>> GetAllAsync();

        Task<CategoryDto> AddAsync(CategoryDto entity);

        Task UpdateAsync(CategoryDto entity);

        Task DeleteAsync(Guid id);
    }
}
