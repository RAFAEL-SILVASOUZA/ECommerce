using ECommerce.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Order.Worker.Infra;

public class PurchaseOrderEntityConfig : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValue(Guid.NewGuid());
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");

        builder.HasMany(x => x.PurchaseOrderItems)
            .WithOne(x => x.PurchaseOrder);
    }
}
