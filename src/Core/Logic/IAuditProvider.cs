namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;

  public interface IAuditProvider
  {
    Task<IAuditTicket> CreateRequestTicket(
      long userId,
      string requestId,
      string ipAddress,
      string requestMethod,
      string userAgent,
      string displayUrl,
      string securityTokenIssuer,
      string securityTokenId);

    Task CloseAuditTicket(long ticketId);

    Task AddHubLog(
      long? auditTicketId,
      string arguments,
      string hubName,
      string method,
      bool isIncoming);

    Task LogRequest(
      IUserContext utx,
      string requestBody);

    Task LogResponse(
      IUserContext utx,
      string responseBody);
  }
}
