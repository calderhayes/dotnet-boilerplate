namespace DotNetBoilerplate.Api.Utility
{
  using System.Linq;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.SignalR.Hubs;
  using Microsoft.Extensions.Logging;

  public class LoggingPipelineModule
    : HubPipelineModule
  {
    /// <summary>
    ///
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    ///
    /// </summary>
    private readonly IApplicationConfiguration applicationConfiguration;

    /// <summary>
    ///
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="applicationConfiguration"></param>
    public LoggingPipelineModule(
      ILoggerFactory loggerFactory,
      IApplicationConfiguration applicationConfiguration)
    {
      this.applicationConfiguration = applicationConfiguration;
      this.logger = loggerFactory.CreateLogger(nameof(LoggingPipelineModule));
    }

    protected override bool OnBeforeIncoming(
      IHubIncomingInvokerContext context)
    {
      if (this.applicationConfiguration.LogWebApiRequests)
      {
        var utx = (IUserContext)context.Hub.Context.Request.HttpContext.Items["UserContext"];

        var args = string.Join(", ", context.Args.Select(a => a.ToString()));
        var hubName = context.MethodDescriptor.Hub.Name;
        var method = context.MethodDescriptor.Name;

        // Not sure how to access DB here
        // this.auditProvider.AddHubLog(
        //   utx.AuditTicketId,
        //   args,
        //   hubName,
        //   method,
        //   true);
        this.logger.LogDebug("=> Invoking " + context.MethodDescriptor.Name + " on hub " + context.MethodDescriptor.Hub.Name);
      }

      return base.OnBeforeIncoming(context);
    }

    protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
    {
      if (this.applicationConfiguration.LogWebApiResponses)
      {
        var hubName = context.Invocation.Hub;
        var methodName = context.Invocation.Method;
        var arguments = string.Join(", ", context.Invocation.Args.Select(o => o.ToString()));

        // Not sure how to access DB here
        // this.auditProvider.AddHubLog(
        //   null,
        //   arguments,
        //   hubName,
        //   methodName,
        //   true);
        this.logger.LogDebug("<= Invoking " + context.Invocation.Method + " on client hub " + context.Invocation.Hub);
      }

      return base.OnBeforeOutgoing(context);
    }
  }
}