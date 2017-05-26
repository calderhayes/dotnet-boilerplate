namespace DotNetBoilerplate.Api.Utility
{
  using System.IO;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;

  /// <summary>
  ///
  /// </summary>
  public class LogResponseMiddleware
  {
    /// <summary>
    ///
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    ///
    /// </summary>
    /// <param name="next"></param>
    public LogResponseMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(
      HttpContext context,
      ILogger<LogResponseMiddleware> logger,
      IAuditProvider auditProvider)
    {
      var userContext = (IUserContext)context.Items["UserContext"];

      var bodyStream = context.Response.Body;

      var responseBodyStream = new MemoryStream();
      context.Response.Body = responseBodyStream;

      await this.next(context);

      responseBodyStream.Seek(0, SeekOrigin.Begin);
      var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

      logger.LogDebug(
        $"RESPONSE LOG: {responseBody}");

      await auditProvider.LogResponse(userContext, responseBody);

      responseBodyStream.Seek(0, SeekOrigin.Begin);
      await responseBodyStream.CopyToAsync(bodyStream);
    }
  }
}
