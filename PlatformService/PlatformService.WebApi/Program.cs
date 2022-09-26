using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformService.MessageBroker.Publisher;
using PlatformService.MessageBroker.Settings;
using PlatformServiceBLL.Mappings;
using PlatformServiceBLL.Services.Concrete;
using PlatformServiceBLL.Services.Interfaces;
using PlatformServiceDAL.Context;
using PlatformServiceDAL.Repositories.Concrete;
using PlatformServiceDAL.Repositories.Interfaces;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddTransient<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton(new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperInitilizer());
        }).CreateMapper());
builder.Host.UseSerilog();
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

app.Run();
