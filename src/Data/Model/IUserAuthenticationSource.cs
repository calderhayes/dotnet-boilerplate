namespace DotNetBoilerplate.Data.Model
{
  /// <summary>
  ///
  /// </summary>
  public interface IUserAuthenticationSource
  {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    string AuthenticationSource { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    string Subject { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    long UserId { get; set; }
  }
}