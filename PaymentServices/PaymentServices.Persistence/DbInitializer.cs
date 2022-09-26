using Microsoft.EntityFrameworkCore;

namespace PaymentService.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(PaymentServiceDbContext context)
        {
            context.Database.EnsureCreated();
            //context.Database.Migrate();
        }
    }
}
