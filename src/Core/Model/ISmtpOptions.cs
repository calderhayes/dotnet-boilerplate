namespace DotNetBoilerplate.Core.Model
{
  public interface ISmtpOptions
  {
    string TargetName { get; }

    string Host { get; }

    int Port { get; }

    string UserName { get; }

    string Password { get; }

    string DefaultFromEmailAddress { get; }
  }
}
