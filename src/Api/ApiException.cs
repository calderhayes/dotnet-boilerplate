namespace DotNetBoilerplate.Api
{
  using System;

  /// <summary>
  ///
  /// </summary>
  public class ApiException
    : Exception
  {
    /// <summary>
    ///
    /// </summary>
    private const string DefaultMessage = "An error occured in the Api";

    /// <summary>
    ///
    /// </summary>
    public ApiException()
      : base(DefaultMessage)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public ApiException(string message)
      : base(message)
    {
    }

    public ApiException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
