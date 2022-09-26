using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain;

namespace PaymentService.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.HasIndex(u => u.UserId).IsUnique();
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
