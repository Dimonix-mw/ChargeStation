using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class DbInitializer
    {
        public static void Initialize(AuthDbContext context)
        {
            //authDbContext.Database.EnsureCreated();
            context.Database.Migrate();
    }
    }
}
