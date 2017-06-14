namespace DotNetBoilerplate.Core.Logic.Email
{
  using System.Threading.Tasks;

  /// <summary>
  ///
  /// </summary>
  public interface IEmailProvider
  {
    Task SendTestEmail();
  }
}
