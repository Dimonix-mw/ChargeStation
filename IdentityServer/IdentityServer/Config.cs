using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients => 
            new Client[] 
            {
                new Client
                {
                    ClientId = "telegramBot",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "telegramBot" }
                }
            };
        public static IEnumerable<ApiScope> ApiScopes => 
            new ApiScope[] 
            { 
                new ApiScope("telegramBot", "telegramBot")
            };
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { };
        public static List<TestUser> TestUsers => new List<TestUser>()
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "user",
                Password = "1234".Sha256()
            }
        };

    }
}
