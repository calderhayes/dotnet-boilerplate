namespace DotNetBoilerplate.Api.Utility
{
  using System;
  using DotNetBoilerplate.Core.Constants;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;
  using Microsoft.Extensions.Logging;

  /// <summary>
  ///
  /// </summary>
  public class GlobalExceptionFilter
    : IExceptionFilter
  {
    /// <summary>
    ///
    /// </summary>
    private readonly ILogger logger;

    private readonly IApplicationConfiguration applicationConfiguration;

    public GlobalExceptionFilter(
      ILoggerFactory loggerFactory,
      IApplicationConfiguration config)
    {
      if (loggerFactory == null)
      {
        throw new ArgumentNullException(nameof(loggerFactory));
      }

      if (config == null)
      {
        throw new ArgumentNullException(nameof(config));
      }

      this.logger = loggerFactory.CreateLogger(nameof(GlobalExceptionFilter));
      this.applicationConfiguration = config;
    }

    public void OnException(ExceptionContext context)
    {
      var userContext = context.HttpContext.Items["UserContext"] as IUserContext;
      var locale = this.applicationConfiguration.DefaultCulture;
      if (userContext != null)
      {
        locale = userContext.UserAccount.Culture;
      }

      var message = "An uncaught exception occurred";

      var exception = this.applicationConfiguration.Environment == EnvironmentType.Development
        ? context.Exception.ToString() : null;

      var response = new
      {
        ResponseId = context.HttpContext.TraceIdentifier,
        Message = message,
        Exception = exception
      };

      context.Result = new ObjectResult(response)
      {
        StatusCode = 500
      };

      this.logger.LogError(0, context.Exception, "An uncaught exception occurred");
    }
  }
}
