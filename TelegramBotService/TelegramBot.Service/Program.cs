using TelegramBot.Service.Services;
using TelegramBotService;
using TelegramBot.Service.Kafka.Common.Publisher;
using TelegramBot.Service.Kafka.Common.Settings;
using Serilog;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(
                    path: hostContext.Configuration["SerilogConfig:SerilogFile"],
                    outputTemplate: hostContext.Configuration["SerilogConfig:SerilogTemplate"],
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        
        var telegram = hostContext.Configuration.GetSection("BotConfiguration");
        var botToken = telegram["BotToken"];

        services.AddSingleton<TelegramBotListener>()
        .AddSingleton<IHostedService, TelegramBotListener>(
            serviceProvider => serviceProvider.GetService<TelegramBotListener>())
        .AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken))
        .AddScoped<IWebApiService, WebApiService>()
        .AddTransient<IKafkaService, KafkaService>()
        .Configure<KafkaSettings>(hostContext.Configuration.GetSection("KafkaSettings"));
        services.AddHttpClient("http-auth", client =>
        {
            client.BaseAddress = new Uri(hostContext.Configuration["BaseAddressAuth"]);
        });
        services.AddHttpClient("http-api", client =>
        {
            client.BaseAddress = new Uri(hostContext.Configuration["BaseAddress"]);
        });
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
