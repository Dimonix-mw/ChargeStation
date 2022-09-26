using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;

namespace PaymentService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(name: "DefaultConnection");
            services.AddDbContext<PaymentServiceDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            services.AddTransient<IPaymentServiceDbContext, PaymentServiceDbContext>();
            //services.AddScoped<IPaymentServiceDbContext, PaymentServiceDbContext>();
            //services.AddScoped<IPaymentServiceDbContext>(provider =>
            //    provider.GetService<PaymentServiceDbContext>());

            return services;
        }
    }
}
