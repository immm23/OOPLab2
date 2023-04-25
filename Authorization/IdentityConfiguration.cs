using IdentityServer4.Models;

namespace Authorization
{
    public class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string>{"role"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("BankAPI.own"),
                new ApiScope("BankAPI.admin")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("BankApi")
                {
                    Scopes = new List<string> { "BankAPI.own", "BankAPI.admin"},
                    ApiSecrets = new List<Secret> { new Secret("BankSecret".Sha256())},
                    UserClaims = new List<string>{"role"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "default",
                    ClientName = "Client Credentials",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("defaultSecret".Sha256()) },
                    AllowedScopes = { "BankAPI.own", "BankAPI.admin"},
                    AllowAccessTokensViaBrowser =true
                }
            };
    }
}
