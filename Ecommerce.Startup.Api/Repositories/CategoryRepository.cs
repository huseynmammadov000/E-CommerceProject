using Azure.Core;
using Ecommerce.Startup.Api.Context;
using Ecommerce.Startup.Api.DTOs;
using Ecommerce.Startup.Api.Exceptions;
using Ecommerce.Startup.Api.Models;
using Ecommerce.Startup.Api.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Startup.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StartupDbContext _dbContext;

        public CategoryRepository(StartupDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoryDto> AddAsync(CategoryDto entity)
        {
            CategoryDto existingCategory = new();
            try
            {
                existingCategory = await this.GetByCategoryNameAsync(entity.Name);
            }
            catch (CategoryNotFoundException) { }

            if (existingCategory.Name != null)
                throw new CategoryAlreadyInUseException();

            var category = entity.Adapt<Category>();


            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();


            return category.Adapt<CategoryDto>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await GetByIdAsync(id);

            var existingCategoryState = _dbContext.Set<Startup.Api.Models.Category>().Local.FirstOrDefault(e => e.Id == id);
            if (existingCategoryState != null)
                _dbContext.Categories.Entry(existingCategoryState).State = EntityState.Detached;

            _dbContext.Categories.Remove(category.Adapt<Startup.Api.Models.Category>());
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync();


            return categories.Adapt<List<CategoryDto>>();
        }

        public async Task<CategoryDto> GetByCategoryNameAsync(string categoryName)
        {
            return (await _dbContext.Categories
            .AsNoTracking()
           .FirstOrDefaultAsync(c => c.Name == categoryName))
           .Adapt<CategoryDto>() ?? throw new CategoryNotFoundException();

        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            return (await _dbContext.Categories
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == id))
              .Adapt<CategoryDto>()
          ?? throw new CategoryNotFoundException();
        }

        public async Task UpdateAsync(CategoryDto entity)
        {
            var existingCategory = await this.GetByIdAsync(entity.Id);

            var existingCategoryState = _dbContext.Set<Startup.Api.Models.Category>().Local.FirstOrDefault(s => s.Id == entity.Id);
            if (existingCategoryState != null)
                _dbContext.Categories.Entry(existingCategoryState).State = EntityState.Detached;

            existingCategory.Id = entity.Id;
            existingCategory.Name = entity.Name;
            

            _dbContext.Categories.Update(existingCategory.Adapt<Startup.Api.Models.Category>());
            await _dbContext.SaveChangesAsync(); 
        }
    }
}
