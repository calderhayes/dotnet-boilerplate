namespace DotNetBoilerplate.Auth.OAuthEntity
{
  using System.Collections.Generic;
  using Microsoft.Extensions.Configuration;

  public static class ScopeFactory
  {
    public static IList<string> GetScopes(IConfigurationRoot config)
    {
      var scopeConfig = config.GetSection("Authentication:Scopes");
      var scopes = new List<string>();
      scopeConfig.Bind(scopes);

      return scopes;
    }
  }
}
