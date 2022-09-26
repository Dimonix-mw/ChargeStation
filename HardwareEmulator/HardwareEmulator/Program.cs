using HardwareEmulator;
using HardwareEmulator.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var pathString = builder.Configuration["SerilogConfig:SerilogFile"];
var outputTemplateString = builder.Configuration["SerilogConfig:SerilogTemplate"];
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Information()
  .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
  .WriteTo.File(
      path: pathString,
      outputTemplate: outputTemplateString,
      rollingInterval: RollingInterval.Day)
  .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<EmulatorService>();
builder.Services.AddSingleton<IHostedService, EmulatorService>(
            serviceProvider => serviceProvider.GetService<EmulatorService>());

builder.Services.Configure<NetSettings>(builder.Configuration.GetSection("NetSettings"));
builder.Services.AddSingleton(Log.Logger);
//builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("https://localhost:44332");
