namespace DotNetBoilerplate.Core.Model
{
  using DotNetBoilerplate.Data.Model;

  public interface IUserContext
  {
    long AuditTicketId { get; }

    string RequestId { get; }

    IUserAccount UserAccount { get; }

    IAuditTicket AuditTicket { get; }

    bool IsAnonymous { get; }
  }
}
