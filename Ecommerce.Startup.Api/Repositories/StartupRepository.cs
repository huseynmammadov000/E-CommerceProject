using Mapster;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

using Ecommerce.Startup.Api.Context;
using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Repositories.Abstractions;
using Ecommerce.Startup.Api.Exceptions;


namespace Ecommerce.Startup.Api.Repositories;

public class StartupRepository(StartupDbContext startupDbContext)
    : IStartupRepository
{
    private readonly StartupDbContext _startupDbContext = Guard.Against.Null(startupDbContext);


    //Exception elave etmek (Startp not Found);
    public async Task<StartupDto> GetByIdAsync(Guid id)
    {
        return (await _startupDbContext.Startups
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id))
            .Adapt<StartupDto>()
        ?? throw new StartupNotFoundException();
    }

    public async Task<List<StartupDto>> GetByCategoryAsync(string category)
    {
        var startups = await _startupDbContext.Startups
            .AsNoTracking()
            .Where(startup => startup.Category.ToString() == category)
            .ToListAsync();

        return startups.Adapt<List<StartupDto>>();
    }

    public async Task<List<StartupDto>> GetByStartupNameAsync(string startupName)
    {
        // Belirli bir startup adı ile eşleşen tüm startup'ları al
        var startups = await _startupDbContext.Startups
            .AsNoTracking()
            .Where(s => s.Name == startupName)
            .ToListAsync();

        // Eğer sonuç boşsa, boş liste döndür
        return startups.Adapt<List<StartupDto>>();
    }

    public async Task<List<StartupDto>> GetAllAsync()
    {
        var startups = await _startupDbContext.Startups
        .AsNoTracking()
        .ToListAsync();

        // Mapster kullanarak Startup nesnelerini StartupDto'lara dönüştür

        return startups.Adapt<List<StartupDto>>();
    }

    public async Task<StartupDto> AddAsync(StartupDto request)
    {
        var startup = request.Adapt<Startup.Api.Models.Startup>();

        await _startupDbContext.Startups.AddAsync(startup);
        await _startupDbContext.SaveChangesAsync();

        return startup.Adapt<StartupDto>();
    }

    public async Task UpdateAsync(StartupDto entity)
    {
        var existingStartup = await this.GetByIdAsync(entity.Id);

        var existingStartupState = _startupDbContext.Set<Startup.Api.Models.Startup>().Local.FirstOrDefault(s => s.Id == entity.Id);
        if (existingStartupState != null)
            _startupDbContext.Startups.Entry(existingStartupState).State = EntityState.Detached;

        existingStartup.Id = entity.Id;
        existingStartup.Name = entity.Name;
        existingStartup.Description = entity.Description;   
        existingStartup.CategoryId = entity.CategoryId;
        existingStartup.UserId = entity.UserId;
        existingStartup.WebAddress = entity.WebAddress;
        existingStartup.Logo = entity.Logo;
        existingStartup.PortfolioId = entity.PortfolioId;
        existingStartup.IsActive = entity.IsActive;

        _startupDbContext.Startups.Update(existingStartup.Adapt<Startup.Api.Models.Startup>());
        await _startupDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var startup = await this.GetByIdAsync(id);

        var existingStartupState = _startupDbContext.Set<Startup.Api.Models.Startup>().Local.FirstOrDefault(e => e.Id == id);
        if (existingStartupState != null)
            _startupDbContext.Startups.Entry(existingStartupState).State = EntityState.Detached;

        _startupDbContext.Startups.Remove(startup.Adapt<Startup.Api.Models.Startup>());
        await _startupDbContext.SaveChangesAsync();
    }
}