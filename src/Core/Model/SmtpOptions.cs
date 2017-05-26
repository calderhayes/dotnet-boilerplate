namespace DotNetBoilerplate.Core.Model
{
  public class SmtpOptions
    : ISmtpOptions
  {
    public bool IsEmailEnabled { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string DefaultFromEmailAddress { get; set; }
  }
}
