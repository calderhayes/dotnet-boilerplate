namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using MailKit.Net.Smtp;
  using Microsoft.Extensions.Logging;
  using MimeKit;

  /// <summary>
  ///
  /// </summary>
  public class EmailProvider
    : IEmailProvider
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    public EmailProvider(
      IApplicationConfiguration config,
      ILogger<EmailProvider> logger)
    {
      this.Configuration = config;
      this.Logger = logger;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IApplicationConfiguration Configuration { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task SendTestEmail()
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("System", this.Configuration.SmtpOptions.DefaultFromEmailAddress));
      message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "caldertest123@mailinator.com"));
      message.Subject = "How you doin'?";

      message.Body = new TextPart("plain")
      {
        Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
      };

      return this.SendMessage(message);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task SendMessage(MimeMessage message)
    {
      var smtpOptions = this.Configuration.SmtpOptions;

      if (!smtpOptions.IsEmailEnabled)
      {
        this.Logger.LogWarning("Emails have been disabled, the following will not be sent: subject {0}", message.Subject);
        return;
      }

      using (var client = new SmtpClient())
      {
        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(smtpOptions.Host, smtpOptions.Port, false);

        // Note: since we don't have an OAuth2 token, disable
        // the XOAUTH2 authentication mechanism.
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        // Note: only needed if the SMTP server requires authentication
        // await client.AuthenticateAsync(smtpOptions.UserName, smtpOptions.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
      }
    }
  }
}
