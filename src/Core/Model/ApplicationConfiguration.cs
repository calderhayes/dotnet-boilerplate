namespace DotNetBoilerplate.Core.Model
{
  using System;
  using System.Collections.Generic;
  using DotNetBoilerplate.Core.Constants;
  using Microsoft.Extensions.Configuration;

  public class ApplicationConfiguration
    : IApplicationConfiguration
  {
    public ApplicationConfiguration(
      IConfigurationRoot configurationRoot)
    {
      this.Configuration = configurationRoot;

      var values = new List<string>();
      this.ApplicationConfigurationSection
        .GetSection("SupportedCultures")
        .Bind(values);
      this.SupportedCultures = values;

      var env = this.ApplicationConfigurationSection.GetValue<string>("Environment");
      switch (env)
      {
        case "Development":
          this.Environment = EnvironmentType.Development;
          break;
        case "Production":
          this.Environment = EnvironmentType.Production;
          break;
        default:
          throw new NotImplementedException($"Enviroment {env} is not recognized");
      }

      values = new List<string>();
      this.ApplicationConfigurationSection
        .GetSection("CorsAllowedOrigins")
        .Bind(values);
      this.CorsAllowedOrigins = values;

      var liveSmtpSection = this.ApplicationConfigurationSection.GetSection("LiveSmtp");
      this.LiveSmtpOptions = BuildSmtpOptions(liveSmtpSection);

      var dummySmtpSection = this.ApplicationConfigurationSection.GetSection("DummySmtp");
      this.DummySmtpOptions = BuildSmtpOptions(dummySmtpSection);
    }

    public EnvironmentType Environment { get; }

    public string LocalDevelopmentTokenIssuer =>
      this.ApplicationConfigurationSection.GetValue<string>("LocalDevelopmentTokenIssuer");

    public bool LogWebApiRequests =>
      this.ApplicationConfigurationSection.GetValue<bool>("LogWebApiRequests");

    public bool LogWebApiResponses =>
      this.ApplicationConfigurationSection.GetValue<bool>("LogWebApiResponses");

    public IList<string> CorsAllowedOrigins { get; }

    public string DefaultCulture
    {
      get
      {
        return this.ApplicationConfigurationSection
          .GetValue<string>("DefaultCulture");
      }
    }

    public IList<string> SupportedCultures { get; }

    public ISmtpOptions LiveSmtpOptions { get; }

    public ISmtpOptions DummySmtpOptions { get; }

    private IConfigurationRoot Configuration { get; }

    private IConfigurationSection ApplicationConfigurationSection =>
      this.Configuration.GetSection("Application");

    /// <summary>
    ///
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    private static SmtpOptions BuildSmtpOptions(IConfigurationSection section)
    {
      return new SmtpOptions(
        section.GetValue<string>("TargetName"),
        section.GetValue<string>("Host"),
        section.GetValue<int>("POrt"),
        section.GetValue<string>("UserName"),
        section.GetValue<string>("Password"),
        section.GetValue<string>("DefaultFromEmailAddress"));
    }
  }
}
