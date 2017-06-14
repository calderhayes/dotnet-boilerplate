namespace DotNetBoilerplate.Core.Logic.Email
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.Extensions.Logging;

  public class EmailProviderFactory
    : IEmailProviderFactory
  {
    public EmailProviderFactory(
      IApplicationConfiguration applicationConfiguration,
      ISecurityProvider securityProvider,
      ILoggerFactory loggerFactory)
    {
      this.ApplicationConfiguration = applicationConfiguration;
      this.SecurityProvider = securityProvider;
      this.LoggerFactory = loggerFactory;
    }

    private IApplicationConfiguration ApplicationConfiguration { get; }

    private ISecurityProvider SecurityProvider { get; }

    private ILoggerFactory LoggerFactory { get; }

    public IEmailProvider Create(IUserContext userContext)
    {
      var isLive = this.SecurityProvider.IsToggleEnabled(
        userContext, SecurityProfileToggleType.SendLiveEmail);
      return this.Create(isLive);
    }

    public async Task<IEmailProvider> Create(long securityProfileId)
    {
      var isLive = await this.SecurityProvider.IsToggleEnabled(
        securityProfileId, SecurityProfileToggleType.SendLiveEmail);
      return this.Create(isLive);
    }

    private IEmailProvider Create(bool isLive)
    {
      if (isLive)
      {
        return new EmailProvider(
          this.ApplicationConfiguration.LiveSmtpOptions,
          this.LoggerFactory.CreateLogger<EmailProvider>());
      }
      else
      {
        return new EmailProvider(
          this.ApplicationConfiguration.DummySmtpOptions,
          this.LoggerFactory.CreateLogger<EmailProvider>());
      }
    }
  }
}