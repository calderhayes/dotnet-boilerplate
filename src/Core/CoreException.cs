namespace DotNetBoilerplate.Core
{
  using System;

  /// <summary>
  ///
  /// </summary>
  public class CoreException
    : Exception
  {
    /// <summary>
    ///
    /// </summary>
    private const string DefaultMessage = "An error occured in the Core library";

    /// <summary>
    ///
    /// </summary>
    public CoreException()
      : base(DefaultMessage)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public CoreException(string message)
      : base(message)
    {
    }

    public CoreException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
