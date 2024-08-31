using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Requests;


namespace Ecommerce.Startup.Api.Services.Abstractions;

public interface IStartupService
{
    Task<StartupDto> AddAsync(CreateStartupRequest entity);
    Task<StartupDto> GetByIdAsync(Guid id);

    Task<List<StartupDto>> GetByStartupNameAsync(string startupName);

    Task<List<StartupDto>> GetByCategoryAsync(string category);

    Task<List<StartupDto>> GetAllAsync();

    Task UpdateAsync(UpdateStartupRequest entity);

    Task DeleteAsync(Guid id);
}