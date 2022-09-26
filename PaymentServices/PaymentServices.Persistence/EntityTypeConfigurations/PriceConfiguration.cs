using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain;

namespace PaymentService.Persistence.EntityTypeConfigurations
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Id).IsUnique();
            builder.HasOne(p => p.Filial)
                .WithMany(f => f.Prices)
                .HasForeignKey(p => p.FilialId);
            builder.HasOne(p => p.PumpModel)
                .WithMany(pm => pm.Prices)
                .HasForeignKey(p => p.PumpModelId);
        }
    }
}