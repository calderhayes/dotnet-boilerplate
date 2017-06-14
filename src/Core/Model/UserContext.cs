namespace DotNetBoilerplate.Core.Model
{
  using System;
  using System.Collections.Generic;
  using DotNetBoilerplate.Data.Model;

  /// <summary>
  ///
  /// </summary>
  public class UserContext
    : IUserContext
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="userAccount"></param>
    /// <param name="auditTicket"></param>
    /// <param name="isAnonymous"></param>
    /// <param name="securityToggles"></param>
    public UserContext(
      IUserAccount userAccount,
      IAuditTicket auditTicket,
      bool isAnonymous,
      IList<ISecurityProfileToggle> securityToggles)
    {
      this.UserAccount = userAccount;
      this.AuditTicket = auditTicket;
      this.IsAnonymous = isAnonymous;
      this.SecurityToggles = securityToggles;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool IsAnonymous { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public long AuditTicketId => this.AuditTicket.TicketId;

    /// <summary>
    ///
    /// </summary>
    public string RequestId => this.AuditTicket.RequestId;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IAuditTicket AuditTicket { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IUserAccount UserAccount { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IList<ISecurityProfileToggle> SecurityToggles { get; }
  }
}
