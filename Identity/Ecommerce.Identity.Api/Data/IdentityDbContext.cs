using Ecommerce.Identity.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Identity.Api.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
    : DbContext(options)
{
    public const string Schema = "Identity";

    public DbSet<Entities.Session> Sessions { get; set; }
    public DbSet<Entities.RefreshToken> RefreshTokens { get; set; }

    public DbSet<Entities.User> Users { get; set; }
    public DbSet<Entities.Role> Roles { get; set; }
    public DbSet<Entities.UserRole> UserRoles { get; set; }


    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
           new Role { Id = Guid.NewGuid(), Name = "Admin" },
           new Role { Id = Guid.NewGuid(), Name = "Entrepreneur" },
           new Role { Id = Guid.NewGuid(), Name = "Investor" }
       );

        modelBuilder.HasDefaultSchema(Schema);
    }
}