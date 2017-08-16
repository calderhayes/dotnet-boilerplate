namespace DotNetBoilerplate.Auth.OAuthEntity
{
  using System.Collections.Generic;
  using IdentityServer4.Models;
  using Microsoft.Extensions.Configuration;

  internal static class ClientFactory
  {
    public static IEnumerable<Client> Get(IConfigurationRoot config)
    {
      var authConfig = config.GetSection("Authentication");

      var scopes = ScopeFactory.GetScopes(config);

      return new List<Client>()
      {
        new Client()
        {
          ClientId = authConfig.GetValue<string>("MainClientId"),
          ClientName = "An example oauth2client",
          AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
          AllowOfflineAccess = true,
          RefreshTokenExpiration = TokenExpiration.Absolute,
          AbsoluteRefreshTokenLifetime = 2592000,
          ClientSecrets = new List<Secret>()
          {
            new Secret(authConfig.GetValue<string>("ClientSecret").Sha512())
          },
          AllowedScopes = scopes,
          AllowedCorsOrigins = new List<string>() { "http://localhost:8080", "http://localhost:8888", "https://localhost:8080" }
        }
      };
    }
  }
}
