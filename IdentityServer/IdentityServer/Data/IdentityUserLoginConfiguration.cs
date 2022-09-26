using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data
{
    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> builder)
        {
            builder.HasNoKey();
            
        }
    }
}
