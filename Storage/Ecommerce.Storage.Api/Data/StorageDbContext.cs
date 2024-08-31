using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Storage.Api.Data;

public sealed class StorageDbContext(DbContextOptions<StorageDbContext> options)
    : DbContext(options)
{
    public const string Schema = "Storage";

    public DbSet<Entities.File> Files { get; set; }


    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }
}