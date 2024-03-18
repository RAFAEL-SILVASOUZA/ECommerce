using ECommerce.Stock.Worker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Stock.Worker.Infra
{
    public class ProductDBContext(DbContextOptions<ProductDBContext> contextOptions,
        IConfiguration configuration) : DbContext(contextOptions)
    {
        private readonly IConfiguration _configuration = configuration;

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CatalogConnection"));
        }
    }
}
