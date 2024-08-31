using Ecommerce.Startup.Api.DTOs;

namespace Ecommerce.Startup.Api.Repositories.Abstractions;

public interface IStartupRepository
{
    Task<StartupDto> GetByIdAsync(Guid id);

    Task<List<StartupDto>> GetByStartupNameAsync(string startupName);

    Task<List<StartupDto>> GetByCategoryAsync(string category);

    Task<List<StartupDto>> GetAllAsync();

    Task<StartupDto> AddAsync(StartupDto entity);

    Task UpdateAsync(StartupDto entity);

    Task DeleteAsync(Guid id);
}