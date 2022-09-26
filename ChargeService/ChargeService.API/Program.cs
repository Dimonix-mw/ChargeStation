using ChargeService.Utility.Extensions;
using Serilog;

namespace ChargeService.API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.FirstCharToUpper() ?? "Development"}.json", optional: false); ;
            IConfiguration config = builder.Build();
            var pathString = config["SerilogConfig:SerilogFile"];
            var outputTemplateString = config["SerilogConfig:SerilogTemplate"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
                .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("aspnetcore-browser-refresh.js")))
                .WriteTo.File(
                    path: pathString,
                    outputTemplate: outputTemplateString,
                    rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}