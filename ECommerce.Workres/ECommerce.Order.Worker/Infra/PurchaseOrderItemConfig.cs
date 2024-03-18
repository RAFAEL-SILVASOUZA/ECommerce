using ECommerce.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Order.Worker.Infra;

public class PurchaseOrderItemConfig : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValue(Guid.NewGuid());
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(x => x.PurchaseOrderItems)
            .HasForeignKey(x => x.OrderEntityId);
    }
}
