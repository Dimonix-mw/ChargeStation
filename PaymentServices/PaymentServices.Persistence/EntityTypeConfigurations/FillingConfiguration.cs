using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain;

namespace PaymentService.Persistence.EntityTypeConfigurations
{
    public class FillingConfiguration : IEntityTypeConfiguration<Filling>
    {
        public void Configure(EntityTypeBuilder<Filling> builder)
        {
            builder.HasKey(f => f.Id);
            builder.HasIndex(f => f.Id).IsUnique();
            //builder.HasOne(f => f.Session)
            //    .WithOne();
        }
    }
}