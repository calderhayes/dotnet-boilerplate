namespace DotNetBoilerplate.Core.Model
{
  public class SmtpOptions
    : ISmtpOptions
  {
    public SmtpOptions()
    {
    }

    public SmtpOptions(
      string targetName,
      string host,
      int port,
      string username,
      string password,
      string defaultFromEmailAddress)
    {
      this.TargetName = targetName;
      this.Host = host;
      this.Port = port;
      this.UserName = username;
      this.Password = password;
      this.DefaultFromEmailAddress = defaultFromEmailAddress;
    }

    public string TargetName { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string DefaultFromEmailAddress { get; set; }
  }
}
