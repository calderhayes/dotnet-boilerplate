namespace DotNetBoilerplate.Auth.OAuthEntity
{
  using System.Collections.Generic;
  using System.Linq;
  using IdentityModel;
  using IdentityServer4.Models;
  using Microsoft.Extensions.Configuration;

  internal static class ResourceFactory
  {
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new List<IdentityResource>()
      {
        new IdentityResources.OpenId(),
          new IdentityResources.Profile(),
          new IdentityResources.Email(),
          new IdentityResource()
          {
            Name = Constants.RoleClaimType,
              Description = "The role in regards to the application",
              UserClaims = new List<string> { Constants.RoleClaimType }
          }
      };
    }

    public static IEnumerable<ApiResource> GetApiResources(IConfigurationRoot config)
    {
      var authConfig = config.GetSection("Authentication");
      var scopes = ScopeFactory.GetScopes(config)
        .Select(s => new Scope(s))
        .ToList();

      return new List<ApiResource>()
      {
        new ApiResource()
        {
          Name = authConfig.GetValue<string>("MainApiResource"),
          DisplayName = "Custom API",
          Description = "Custom API access",
          UserClaims = new List<string>()
          {
            Constants.RoleClaimType,
            JwtClaimTypes.Email,
            JwtClaimTypes.PreferredUserName,
            JwtClaimTypes.AuthenticationTime,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.Subject,
            JwtClaimTypes.Locale
          },
          ApiSecrets = new List<Secret>()
          {
            new Secret(authConfig.GetValue<string>("Secret").Sha512())
          },
          Scopes = scopes
        }
      };
    }
  }
}
