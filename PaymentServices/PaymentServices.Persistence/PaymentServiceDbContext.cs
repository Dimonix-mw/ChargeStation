using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;
using PaymentService.Persistence.EntityTypeConfigurations;
#pragma warning disable CS8618

namespace PaymentService.Persistence
{
    public class PaymentServiceDbContext : DbContext, IPaymentServiceDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Filial> Filials { get; set; }
        public DbSet<Filling> Fillings { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<PumpModel> PumpModels { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new PriceConfiguration());
            builder.ApplyConfiguration(new PumpConfiguration());
            builder.ApplyConfiguration(new SessionConfiguration());
            builder.ApplyConfiguration(new FillingConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
