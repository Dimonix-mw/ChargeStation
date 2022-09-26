using IdentityServer;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AuthDbContext>(options =>
                  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole<long>>(config => 
    {
        config.Password.RequiredLength = 6;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireDigit = false;
        config.Password.RequireUppercase = false;
    }
).AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders()
;
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryClients(Config.Clients)
    //.AddInMemoryIdentityResources(Config.IdentityResources)
    //.AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    //.AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();
app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("https://localhost:8001");
