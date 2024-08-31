using Ecommerce.Startup.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Startup.Api.Context
{
    public class StartupDbContext(DbContextOptions<StartupDbContext> options) :DbContext(options)
    {
        public const string Schema = "Startup";

        public DbSet<Category> Categories { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Radar> Radars { get; set; }
        public DbSet<Models.Startup> Startups { get; set; }

        protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
