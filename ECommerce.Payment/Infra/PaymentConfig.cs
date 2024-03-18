using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaymentModel = ECommerce.Payment.Domain.Entities.Payment;

namespace ECommerce.Payment.Infra
{
    public class PaymentConfig : IEntityTypeConfiguration<PaymentModel>
    {
        public void Configure(EntityTypeBuilder<PaymentModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValue(Guid.NewGuid());
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        }
    }
}
