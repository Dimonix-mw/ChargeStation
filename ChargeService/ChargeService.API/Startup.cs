using AutoMapper;
using ChargeService.API.Middlewares;
using ChargeService.BLL.Mappings;
using ChargeService.BLL.Services.Concrete;
using ChargeService.BLL.Services.Interfaces;
using ChargeService.DAL.Context;
using ChargeService.DAL.Repositories.Concrete;
using ChargeService.DAL.Repositories.Interfaces;
using ChargeService.MessageBroker.Publisher;
using ChargeService.MessageBroker.Settings;
using Microsoft.EntityFrameworkCore;

namespace ChargeService.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISessionService, SessionService>();

            services.AddTransient<IRabbitMqService, RabbitMqService>();
            services.Configure<RabbitMQSettings>(_configuration.GetSection("RabbitMQSettings"));

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperInitilizer());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
