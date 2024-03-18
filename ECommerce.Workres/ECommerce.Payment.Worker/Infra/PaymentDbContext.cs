using Microsoft.EntityFrameworkCore;

namespace ECommerce.Payment.Worker.Infra
{
    public class PaymentDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Domain.Entities.Payment> Payments { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> contextOptions,
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
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("PaymentConnection"));
        }
    }
}
