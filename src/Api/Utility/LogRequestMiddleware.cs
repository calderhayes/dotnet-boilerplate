namespace DotNetBoilerplate.Api.Utility
{
  using System.IO;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Http.Extensions;
  using Microsoft.Extensions.Logging;

  public class LogRequestMiddleware
  {
    /// <summary>
    ///
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    ///
    /// </summary>
    /// <param name="next"></param>
    public LogRequestMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="auditProvider"></param>
    /// <returns></returns>
    public async Task Invoke(
      HttpContext context,
      ILogger<LogRequestMiddleware> logger,
      IAuditProvider auditProvider)
    {
      var userContext = (IUserContext)context.Items["UserContext"];

      var requestBodyStream = new MemoryStream();
      var originalRequestBody = context.Request.Body;

      await context.Request.Body.CopyToAsync(requestBodyStream);
      requestBodyStream.Seek(0, SeekOrigin.Begin);

      var url = UriHelper.GetDisplayUrl(context.Request);
      var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

      var message = $"REQUEST METHOD: {context.Request.Method}, REQUEST BODY: {requestBodyText}, REQUEST URL: {url}";
      logger.LogDebug(
          message);

      await auditProvider.LogRequest(
        userContext,
        requestBodyText);

      requestBodyStream.Seek(0, SeekOrigin.Begin);
      context.Request.Body = requestBodyStream;

      await this.next(context);
      context.Request.Body = originalRequestBody;
    }
  }
}
