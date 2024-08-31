using Ardalis.GuardClauses;
using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Integration.Services.Abstractions;
using Ecommerce.Startup.Api.Models;
using Ecommerce.Startup.Api.Repositories.Abstractions;
using Ecommerce.Startup.Api.Requests;
using Ecommerce.Startup.Api.Services.Abstractions;
using Mapster;

namespace Ecommerce.Startup.Api.Services;

public class StartupService(IStartupRepository startupRepository, IIntegrationIdentityClientService integrationIdentityClientService,ICategoryRepository categoryRepository)
    : IStartupService
{
    private readonly IStartupRepository _startupRepository = Guard.Against.Null(startupRepository);
    private readonly ICategoryRepository _categoryRepository = Guard.Against.Null(categoryRepository);
    private readonly IIntegrationIdentityClientService _identityClientService = Guard.Against.Null(integrationIdentityClientService);


    public async Task<StartupDto> AddAsync(CreateStartupRequest entity)
    {
        var currentUserIdResponse = await _identityClientService.GetCurrentUserIdAsync();

        var category =  await _categoryRepository.GetByCategoryNameAsync(entity.CategoryName);

        StartupDto result = new()
        {
            Name = entity.Name,
            Description = entity.Description,
            UserId = currentUserIdResponse.Id,
            Logo = entity.Logo,
            //PortfolioId = entity.PortfolioId,
            CategoryId =category.Id,
            FoundationYear = entity.FoundationYear,
            IsActive = entity.IsActive,
            WebAddress = entity.WebAddress,

            // TODO: map all props
        };

        return await _startupRepository.AddAsync(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _startupRepository.DeleteAsync(id);
    }

    public async Task<List<StartupDto>> GetAllAsync()
    {
        var startups = await _startupRepository.GetAllAsync();
        return startups;
    }

    public async Task<List<StartupDto>> GetByCategoryAsync(string category)
    {
        var startupsByCategory = await _startupRepository.GetByCategoryAsync(category);
        return startupsByCategory;
    }

    public async Task<StartupDto> GetByIdAsync(Guid id)
    {
        var startup = await _startupRepository.GetByIdAsync(id);
        return startup;
    }

    public async Task<List<StartupDto>> GetByStartupNameAsync(string startupName)
    {
        var startup = await _startupRepository.GetByStartupNameAsync(startupName);
        return startup;
    }

    public async Task UpdateAsync(UpdateStartupRequest entity)
    {
        StartupDto updatedStartup = new()
        {
            Name = entity.Name,
            Description = entity.Description,
            UserId = entity.UserId,
            Logo = entity.Logo,
            PortfolioId = entity.PortfolioId,
            CategoryId = entity.CategoryId,
            FoundationYear = entity.FoundationYear,
            IsActive = entity.IsActive,
            WebAddress = entity.WebAddress,

            
        };
        var startup = updatedStartup.Adapt<StartupDto>();
        await _startupRepository.UpdateAsync(startup);
    }
}