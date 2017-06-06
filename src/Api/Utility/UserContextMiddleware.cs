namespace DotNetBoilerplate.Api.Utility
{
  using System;
  using System.Linq;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Constants;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Http.Extensions;
  using Microsoft.Extensions.Logging;

  public class UserContextMiddleware
  {
    /// <summary>
    ///
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    ///
    /// </summary>
    private readonly ILogger<UserContextMiddleware> logger;

    public UserContextMiddleware(
      RequestDelegate next,
      ILogger<UserContextMiddleware> logger)
    {
      this.next = next;
      this.logger = logger;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="accountProvider"></param>
    /// <param name="auditProvider"></param>
    /// <param name="applicationConfigurtaion"></param>
    /// <returns></returns>
    public async Task Invoke(
      HttpContext context,
      IAccountProvider accountProvider,
      IAuditProvider auditProvider,
      IApplicationConfiguration applicationConfigurtaion)
    {
      var userContext = await this.CreateUserContext(
        context,
        accountProvider,
        auditProvider,
        applicationConfigurtaion);

      this.logger.LogInformation("User context has been created");

      context.Items.Add("UserContext", userContext);

      var culture = userContext.UserAccount.Culture;
      context.Response.Headers["Content-Language"] = culture;
      await this.next(context);

      this.logger.LogInformation("Closing user context");
      await auditProvider.CloseAuditTicket(userContext.AuditTicketId);
      context.Items.Remove("UserContext");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="accountProvider"></param>
    /// <param name="auditProvider"></param>
    /// <param name="applicationConfigurtaion"></param>
    /// <returns></returns>
    private async Task<UserContext> CreateUserContext(
      HttpContext context,
      IAccountProvider accountProvider,
      IAuditProvider auditProvider,
      IApplicationConfiguration applicationConfigurtaion)
    {
      var claimsPrincipal = context.User;

      string authSource = string.Empty;
      string authSubjectId = string.Empty;

      var ipAddress = RequestFunctions.GetRequestIP(context, true);

      IUserAccount userAccount = null;
      var isAnonymous = true;

      if (claimsPrincipal.Identity.IsAuthenticated)
      {
        // Get from claims principal
        var issuerClaim = claimsPrincipal.Claims
          .Where(c => c.Type == "iss")
          .SingleOrDefault();

        if (issuerClaim == null)
        {
          throw new ApiException("Could not find the iss claim");
        }

        authSource = this.GetAuthenticationSource(
          issuerClaim.Value,
          applicationConfigurtaion);

        var subClaim = claimsPrincipal.Claims
          .Where(c => c.Type == ClaimTypes.NameIdentifier)
          .SingleOrDefault();

        if (subClaim == null)
        {
          throw new ApiException($"Could not find the {ClaimTypes.NameIdentifier} claim");
        }

        authSubjectId = subClaim.Value;

        userAccount = await accountProvider.GetUserByAuthSourceAndSub(authSource, authSubjectId);

        if (userAccount == null)
        {
          // Register
          var anon = await accountProvider.GetAnonymousAccount();
          var ticket = await auditProvider.CreateRequestTicket(
            anon.Id,
            context.TraceIdentifier,
            ipAddress,
            context.Request.Method,
            RequestFunctions.GetHeaderValueAs<string>(context, "User-Agent"),
            UriHelper.GetDisplayUrl(context.Request),
            authSource,
            authSubjectId);

          this.logger.LogDebug("Attempting to register a new user");

          var preferredUsername = claimsPrincipal
            .Claims
            .Where(c => c.Type == "preferred_username")
            .Select(c => c.Value)
            .SingleOrDefault();

          var email = claimsPrincipal
            .Claims
            .Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value)
            .SingleOrDefault();

          var roles = claimsPrincipal
            .Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

          var locale = claimsPrincipal
            .Claims
            .Where(c => c.Type == "locale")
            .Select(c => c.Value)
            .SingleOrDefault();

          locale = locale ?? this.GetCulture(context, applicationConfigurtaion);

          // Final check to ensure it is valid
          locale = applicationConfigurtaion.SupportedCultures
            .Any(s => s.Equals(locale, StringComparison.CurrentCultureIgnoreCase))
            ? locale : applicationConfigurtaion.DefaultCulture;

          userAccount = await accountProvider.RegisterUser(
            ticket.TicketId,
            authSource,
            authSubjectId,
            preferredUsername,
            email,
            locale,
            roles);
        }

        isAnonymous = false;
      }
      else
      {
        var culture = this.GetCulture(context, applicationConfigurtaion);
        userAccount = await accountProvider.GetAnonymousAccount();
        userAccount.Culture = culture;
        isAnonymous = true;
      }

      var requestTicket = await auditProvider.CreateRequestTicket(
        userAccount.Id,
        context.TraceIdentifier,
        ipAddress,
        context.Request.Method,
        RequestFunctions.GetHeaderValueAs<string>(context, "User-Agent"),
        UriHelper.GetDisplayUrl(context.Request),
        authSource,
        authSubjectId);

      return new UserContext(userAccount, requestTicket, isAnonymous);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="applicationConfigurtaion"></param>
    /// <returns></returns>
    private string GetCulture(
      HttpContext context,
      IApplicationConfiguration applicationConfigurtaion)
    {
      if (!context.Request.Headers.ContainsKey("Accept-Language"))
      {
        return applicationConfigurtaion.DefaultCulture;
      }
      else
      {
        var rawCultures = context.Request.Headers["Accept-Language"];
        try
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
            var match = applicationConfigurtaion.SupportedCultures
              .Where(s => s.Equals(vw.Value, StringComparison.OrdinalIgnoreCase))
              .FirstOrDefault();

            if (match != null)
            {
              return match;
            }

            var language = vw.Value.Substring(0, 2);
            match = applicationConfigurtaion.SupportedCultures
              .Where(s => s.Substring(0, 2).Equals(language, StringComparison.OrdinalIgnoreCase))
              .FirstOrDefault();

            if (match != null)
            {
              return match;
            }
          }

          return applicationConfigurtaion.DefaultCulture;
        }
        catch (Exception ex)
        {
          this.logger.LogWarning(0, ex, "Error deriving user culture from Accept-Language");
          return applicationConfigurtaion.DefaultCulture;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="issuer"></param>
    /// <param name="applicationConfigurtaion"></param>
    /// <returns></returns>
    private string GetAuthenticationSource(
      string issuer, IApplicationConfiguration applicationConfigurtaion)
    {
      if (issuer.Equals(applicationConfigurtaion.LocalDevelopmentTokenIssuer, StringComparison.OrdinalIgnoreCase))
      {
        return AuthenticationSource.LocalDevelopment;
      }
      else
      {
        throw new NotImplementedException($"The issuer {issuer} has not been configured for this system");
      }
    }
  }
}