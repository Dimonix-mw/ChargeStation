using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain;

namespace PaymentService.Persistence.EntityTypeConfigurations
{
    public class PumpConfiguration : IEntityTypeConfiguration<Pump>
    {
        public void Configure(EntityTypeBuilder<Pump> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Id).IsUnique();
            builder.HasOne(p => p.Model)
                .WithMany(m => m.Pumps)
                .HasForeignKey(p => p.ModelId);
            builder.HasOne(p => p.Filial)
                .WithMany(f => f.Pumps)
                .HasForeignKey(p => p.FilialId);
        }
    }
}