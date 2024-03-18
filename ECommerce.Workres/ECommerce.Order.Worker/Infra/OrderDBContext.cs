using ECommerce.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Order.Worker.Infra;

public class OrderDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> contextOptions,
        IConfiguration configuration) : base(contextOptions)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("OrderConnection"));
    }
}
