using PaymentService.Application;
using PaymentService.Application.Common.Mappings;
using PaymentService.MessageBrocker.Consumer.Services;
using PaymentServices.Application.Interfaces;
using PaymentServices.Persistence;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => 
        services.AddAutoMapper(config => {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IPaymentServiceDbContext).Assembly));
        })
        .AddApplication()
        .AddPersistence(hostContext.Configuration)
        .AddHostedService<ListenerRabbitMQService>())
    .Build();

/*var serviceProvider = services.BuildServiceProvider();
try
{
    var context = serviceProvider.GetRequiredService<PaymentServiceDbContext>();
    DbInitializer.Initialize(context);
}
catch (Exception)
{
    throw;
}*/

await host.RunAsync();

