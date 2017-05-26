namespace DotNetBoilerplate.Api.Utility
{
  using System;
  using DotNetBoilerplate.Core.Constants;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.SignalR.Hubs;
  using Microsoft.Extensions.Logging;

  public class GlobalExceptionHubModule
    : HubPipelineModule
  {
    /// <summary>
    ///
    /// </summary>
    private readonly ILogger logger;

    private readonly IApplicationConfiguration applicationConfiguration;

    public GlobalExceptionHubModule(
      ILoggerFactory loggerFactory,
      IApplicationConfiguration config)
    {
      this.logger = loggerFactory.CreateLogger(nameof(GlobalExceptionHubModule));
      this.applicationConfiguration = config;
    }

    protected override void OnIncomingError(
      ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
    {
      var msg = "An unhandled exception occurred";
      this.logger.LogError(0, exceptionContext.Error, msg);

      if (this.applicationConfiguration.Environment == EnvironmentType.Development)
      {
        exceptionContext.Error = new Exception(msg);
      }

      base.OnIncomingError(exceptionContext, invokerContext);
    }
  }
}