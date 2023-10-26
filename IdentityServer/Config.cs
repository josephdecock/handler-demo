using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "par-default-handler",
                ClientName = "Sample PAR Client using the default oidc handler",

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RequireRequestObject = false,

                RequireConsent = true,
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:6001/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:6001/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                }
            },

            new Client
            {
                ClientId = "par-customized-handler",
                ClientName = "Sample PAR Client using the customized oidc handler",

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RequireRequestObject = false,

                RequireConsent = true,
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:6002/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:6002/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:6002/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                }
            },
        };
}
