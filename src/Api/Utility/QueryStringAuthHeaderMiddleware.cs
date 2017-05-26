namespace DotNetBoilerplate.Api.Utility
{
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;

  /// <summary>
  ///
  /// </summary>
  public class QueryStringAuthHeaderMiddleware
  {
    /// <summary>
    ///
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    ///
    /// </summary>
    /// <param name="next"></param>
    public QueryStringAuthHeaderMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(
      HttpContext context,
      ILogger<QueryStringAuthHeaderMiddleware> logger)
    {
      if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
      {
        if (context.Request.QueryString.HasValue)
        {
          var token = context.Request.QueryString.Value
            .Split('&')
            .SingleOrDefault(x => x.Contains("authorization"))?.Split('=')[1];

          if (!string.IsNullOrWhiteSpace(token))
          {
            context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
          }
        }
      }

      await this.next(context);
    }
  }
}
