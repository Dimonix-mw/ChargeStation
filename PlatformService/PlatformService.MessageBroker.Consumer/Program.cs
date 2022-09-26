using PlatformService.MessageBroker.Consumer.Services;
using Microsoft.EntityFrameworkCore;
using PlatformService.MessageBroker.Settings;
using PlatformServiceDAL.Context;
using PlatformServiceDAL.Repositories.Concrete;
using PlatformServiceDAL.Repositories.Interfaces;
using PlatformService.MessageBroker.Publisher;
using PlatformServiceBLL.Services.Concrete;
using PlatformServiceBLL.Services.Interfaces;
using AutoMapper;
using PlatformServiceBLL.Mappings;
using Serilog;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        var pathString = hostContext.Configuration["SerilogConfig:SerilogFile"];
        var outputTemplateString = hostContext.Configuration["SerilogConfig:SerilogTemplate"];
        Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
          .WriteTo.File(
              path: pathString,
              outputTemplate: outputTemplateString,
              rollingInterval: RollingInterval.Day)
          .CreateLogger();

        services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")))
        .AddTransient<IUnitOfWork, UnitOfWork>()
        .AddTransient<ISessionService, SessionService>()
        .Configure<RabbitMQSettings>(hostContext.Configuration.GetSection("RabbitMQSettings"))
        .AddHostedService<ListenerChargeServiceRabbitMQ>()
        .AddHostedService<ListenerPaymentServiceRabbitMQ>()
        .AddTransient<IRabbitMqService, RabbitMqService>()
        .AddScoped<IWebApiService, WebApiService>()
        .AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperInitilizer());
            }).CreateMapper())
        .AddHttpClient("http-api", client =>
            {
                client.BaseAddress = new Uri(hostContext.Configuration["BaseAddress"]);
            });
    })
    .UseSerilog()
    .Build();
await host.RunAsync();

