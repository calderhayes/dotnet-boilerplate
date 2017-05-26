namespace DotNetBoilerplate.Core.Logic
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.Extensions.Logging;

  /// <summary>
  /// Cannot have UserContextFactory as a DI dependency!
  /// </summary>
  public class AuditProvider
    : IAuditProvider
  {
    public AuditProvider(
      ILogger<AuditProvider> logger,
      IDbContext dbContext)
    {
      this.Logger = logger;
      this.DbContext = dbContext;
    }

    private IDbContext DbContext { get; }

    private ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="requestId"></param>
    /// <param name="ipAddress"></param>
    /// <param name="requestMethod"></param>
    /// <param name="userAgent"></param>
    /// <param name="displayUrl"></param>
    /// <param name="securityTokenIssuer"></param>
    /// <param name="securityTokenId"></param>
    /// <returns></returns>
    public async Task<IAuditTicket> CreateRequestTicket(
      long userId,
      string requestId,
      string ipAddress,
      string requestMethod,
      string userAgent,
      string displayUrl,
      string securityTokenIssuer,
      string securityTokenId)
    {
      var ticket = new AuditTicket()
      {
        UserId = userId,
        RequestId = requestId,
        IpAddress = ipAddress,
        RequestMethod = requestMethod,
        UserAgent = userAgent,
        DisplayUrl = displayUrl,
        SecurityTokenIssuer = securityTokenIssuer,
        SecurityTokenId = securityTokenId,
        StartTime = DateTimeOffset.Now
      };

      this.DbContext.AuditTickets.Add(ticket);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);

      return ticket;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ticketId"></param>
    /// <returns></returns>
    public async Task CloseAuditTicket(long ticketId)
    {
      var ticket = this.DbContext.AuditTickets
        .Local.SingleOrDefault(t => t.TicketId == ticketId);

      if (ticket == null)
      {
        ticket = new AuditTicket();
        ticket.TicketId = ticketId;
        this.DbContext.AuditTickets.Attach(ticket);
      }

      ticket.EndTime = DateTimeOffset.Now;

      this.DbContext.Entry(ticket).Property(a => a.EndTime).IsModified = true;
      await this.DbContext.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="auditTicketId"></param>
    /// <param name="arguments"></param>
    /// <param name="hubName"></param>
    /// <param name="method"></param>
    /// <param name="isIncoming"></param>
    /// <returns></returns>
    public async Task AddHubLog(
      long? auditTicketId,
      string arguments,
      string hubName,
      string method,
      bool isIncoming)
    {
      var hubLog = new HubLog()
      {
        AuditTicketId = auditTicketId,
        Arguments = arguments,
        HubName = hubName,
        MethodName = method,
        IsIncoming = isIncoming
      };

      this.DbContext.HubLogs.Add(hubLog);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="utx"></param>
    /// <param name="requestBody"></param>
    /// <returns></returns>
    public async Task LogRequest(
      IUserContext utx,
      string requestBody)
    {
      var requestLog = new RequestLog()
      {
        AuditTicketId = utx.AuditTicketId,
        RequestBody = requestBody
      };

      this.DbContext.RequestLogs.Add(requestLog);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="utx"></param>
    /// <param name="responseBody"></param>
    /// <returns></returns>
    public async Task LogResponse(
      IUserContext utx,
      string responseBody)
    {
      var responseLog = new ResponseLog()
      {
        AuditTicketId = utx.AuditTicketId,
        ResponseBody = responseBody
      };

      this.DbContext.ResponseLogs.Add(responseLog);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);
    }
  }
}
