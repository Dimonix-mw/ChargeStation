using AutoMapper;
using ChargeService.BLL.Mappings;
using ChargeService.BLL.Services.Concrete;
using ChargeService.BLL.Services.Interfaces;
using ChargeService.DAL.Context;
using ChargeService.DAL.Repositories.Concrete;
using ChargeService.DAL.Repositories.Interfaces;
using ChargeService.Kafka.Common.Publisher;
using ChargeService.Kafka.Common.Settings;
using ChargeService.MessageBroker.Consumer.Services;
using ChargeService.MessageBroker.Settings;
using ChargeService.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var host = new HostBuilder()
          .ConfigureHostConfiguration(configHost => {
          })
          .ConfigureServices((hostContext, services) => {
              var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.FirstCharToUpper() ?? "Development"}.json", optional: false);
              IConfiguration config = builder.Build();
              var connectionString = config.GetConnectionString("DefaultConnection");

              var pathString = config["SerilogConfig:SerilogFile"];
              var outputTemplateString = config["SerilogConfig:SerilogTemplate"];
              Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(
                    path: pathString,
                    outputTemplate: outputTemplateString,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

              services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseNpgsql(connectionString));
              services.AddTransient<IUnitOfWork, UnitOfWork>();
              services.AddTransient<ISessionService, SessionService>();
              services.AddTransient<IKafkaService, KafkaService>();
              services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));
              services.Configure<KafkaSettings>(config.GetSection("KafkaSettings"));
              services.AddHostedService<UpdateRequestMQStatusConsumer>();

              var mapperConfig = new MapperConfiguration(mc =>
              {
                  mc.AddProfile(new MapperInitilizer());
              });
              IMapper mapper = mapperConfig.CreateMapper();
              services.AddSingleton(mapper);
          })
         .UseSerilog()
         .UseConsoleLifetime()
         .Build();

//run the host
host.Run();

