using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public AuthDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<long>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<long>>(entity => entity.ToTable(name: "UserClaims"));
            builder.Entity<IdentityUserLogin<long>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<long>>(entity => entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));

            //builder.ApplyConfiguration();

        }
    }
}
