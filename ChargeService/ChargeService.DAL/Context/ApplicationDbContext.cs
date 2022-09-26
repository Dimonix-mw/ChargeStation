using ChargeService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChargeService.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasKey(x => x.Id);
            
            modelBuilder.HasSequence<int>("SessionNumbers")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Session>()
                        .Property(o => o.Id)
                        .HasDefaultValueSql("nextval('\"SessionNumbers\"')");


            modelBuilder.Entity<Filling>()
                .HasKey(x => x.Id);

            modelBuilder.HasSequence<int>("FillingNumbers")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Filling>()
                        .Property(o => o.Id)
                        .HasDefaultValueSql("nextval('\"FillingNumbers\"')");


            modelBuilder.Entity<Filling>()
                .HasOne(f => f.Session)
                .WithOne(s => s.Filling)
                .HasForeignKey<Session>(s => s.Id);
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Filling> Fillings { get; set; }
    }
}
