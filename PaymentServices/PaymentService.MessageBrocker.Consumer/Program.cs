using PaymentService.Application;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Common.Mappings;
using PaymentService.MessageBrocker.Consumer.Services;
using PaymentService.MessageBroker.Common.Settings;
using PaymentService.MessageBroker.Common.Publisher;
using PaymentService.Persistence;
using System.Reflection;
using Serilog;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(
                    path: hostContext.Configuration["SerilogConfig:SerilogFile"],
                    outputTemplate: hostContext.Configuration["SerilogConfig:SerilogTemplate"],
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IPaymentServiceDbContext).Assembly));
        })
        .AddApplication()
        .AddPersistence(hostContext.Configuration)
        .AddTransient<IRabbitMqService, RabbitMqService>()
        .Configure<RabbitMQSettings>(hostContext.Configuration.GetSection("RabbitMQSettings"))
        .AddHostedService<ListenerRabbitMQService>();
     })
    .UseSerilog()
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();