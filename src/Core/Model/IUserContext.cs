namespace DotNetBoilerplate.Core.Model
{
  using System.Collections.Generic;
  using DotNetBoilerplate.Data.Model;

  public interface IUserContext
  {
    long AuditTicketId { get; }

    string RequestId { get; }

    IUserAccount UserAccount { get; }

    IAuditTicket AuditTicket { get; }

    bool IsAnonymous { get; }

    IList<ISecurityProfileToggle> SecurityToggles { get; }
  }
}
