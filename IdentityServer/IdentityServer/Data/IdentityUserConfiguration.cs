using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUser<long>> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
