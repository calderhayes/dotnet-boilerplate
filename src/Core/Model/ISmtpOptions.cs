namespace DotNetBoilerplate.Core.Model
{
  public interface ISmtpOptions
  {
    bool IsEmailEnabled { get; }

    string Host { get; }

    int Port { get; }

    string UserName { get; }

    string Password { get; }

    string DefaultFromEmailAddress { get; }
  }
}
