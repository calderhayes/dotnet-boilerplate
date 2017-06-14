namespace DotNetBoilerplate.Api.Utility
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Primitives;

  public static class RequestFunctions
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="tryUseXForwardHeader"></param>
    /// <returns></returns>
    public static string GetRequestIP(
      HttpContext context, bool tryUseXForwardHeader)
    {
      string ip = null;

      if (tryUseXForwardHeader)
      {
        ip = GetHeaderValueAs<string>(
          context, "X-Forwarded-For").SplitCsv().FirstOrDefault();
      }

      // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
      if (string.IsNullOrWhiteSpace(ip) && context?.Connection?.RemoteIpAddress != null)
      {
        ip = context.Connection.RemoteIpAddress.ToString();
      }

      if (string.IsNullOrWhiteSpace(ip))
      {
        ip = GetHeaderValueAs<string>(context, "REMOTE_ADDR");
      }

      // context?.Request?.Host this is the local host.
      if (string.IsNullOrWhiteSpace(ip))
      {
        return null;
      }

      return ip;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="csvList"></param>
    /// <param name="nullOrWhitespaceInputReturnsNull"></param>
    /// <returns></returns>
    public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
    {
      if (string.IsNullOrWhiteSpace(csvList))
      {
        return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
      }

      return csvList
        .TrimEnd(',')
        .Split(',')
        .AsEnumerable<string>()
        .Select(s => s.Trim())
        .ToList();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="headerName"></param>
    /// <typeparam name="T">Value</typeparam>
    /// <returns></returns>
    public static T GetHeaderValueAs<T>(HttpContext context, string headerName)
    {
      StringValues values;

      if (context?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
      {
        // writes out as Csv when there are multiple.
        string rawValues = values.ToString();

        if (!string.IsNullOrEmpty(rawValues))
        {
          return (T)Convert.ChangeType(rawValues, typeof(T));
        }
      }

      return default(T);
    }

    public static string ParseCulture(
      StringValues rawCultures, IList<string> supportedCultures)
    {
      var valuesAndWeights = rawCultures
        .Select(v =>
        {
          var split = v.Split(';');
          return new
          {
            Value = split[0],
            Weight = split.Length > 1 ? decimal.Parse(split[1]) : 1M
          };
        })
        .OrderByDescending(v => v.Weight);

      foreach (var vw in valuesAndWeights)
      {
        var match = supportedCultures
          .Where(s => s.Equals(vw.Value, StringComparison.OrdinalIgnoreCase))
          .FirstOrDefault();

        if (match != null)
        {
          return match;
        }

        var language = vw.Value.Substring(0, 2);
        match = supportedCultures
          .Where(s => s.Substring(0, 2).Equals(language, StringComparison.OrdinalIgnoreCase))
          .FirstOrDefault();

        if (match != null)
        {
          return match;
        }
      }

      return null;
    }
  }
}
