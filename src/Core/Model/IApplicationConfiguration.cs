namespace DotNetBoilerplate.Core.Model
{
  using System.Collections.Generic;
  using DotNetBoilerplate.Core.Constants;

  public interface IApplicationConfiguration
  {
    EnvironmentType Environment { get; }

    string LocalDevelopmentTokenIssuer { get; }

    bool LogWebApiRequests { get; }

    bool LogWebApiResponses { get; }

    IList<string> CorsAllowedOrigins { get; }

    string DefaultCulture { get; }

    IList<string> SupportedCultures { get; }

    ISmtpOptions SmtpOptions { get; }
  }
}
