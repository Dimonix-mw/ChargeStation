using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentServiceDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Filial> Filials { get; set; }
        DbSet<Filling> Fillings { get; set; }
        DbSet<Price> Prices { get; set; }
        DbSet<Pump> Pumps { get; set; }
        DbSet<PumpModel> PumpModels { get; set; }
        DbSet<Session> Sessions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
