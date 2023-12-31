﻿using IdentityServer4.Models;
using IdentityServer4;

namespace DaprApp.SSO
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile }
                },
                
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "http://127.0.0.1:5173/login.html","http://127.0.0.1/login.html","http://localhost/login.html" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://127.0.0.1:5173/signout-callback-oidc","http://127.0.0.1/signout-callback-oidc","http://localhost/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:32769/swagger/oauth2-redirect.html" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://127.0.0.1:5173/signout-callback-oidc","http://127.0.0.1/signout-callback-oidc","http://localhost/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
            };
    }
}
