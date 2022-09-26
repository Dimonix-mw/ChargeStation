using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain;

namespace PaymentService.Persistence.EntityTypeConfigurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.Id).IsUnique();
            builder.HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId);
            builder.HasOne(s => s.Filling)
                .WithOne();
            builder.Property(s => s.RequestId).IsRequired();
        }
    }
}