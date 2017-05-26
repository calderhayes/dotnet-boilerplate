namespace DotNetBoilerplate.Core.Logic
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
