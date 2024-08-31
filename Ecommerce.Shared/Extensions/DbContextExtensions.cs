using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Ecommerce.Shared.Options;


namespace Ecommerce.Shared.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddSqlServerDbContext<TContext>(this IServiceCollection services, DbConfig dbConfig)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(dbConfig.SQLServer);
        });

        return services;
    }
}