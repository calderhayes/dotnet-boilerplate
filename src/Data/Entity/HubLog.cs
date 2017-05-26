namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;

  public class HubLog
    : IHubLog
  {
    [Key]
    public long HubLogId { get; set; }

    public long? AuditTicketId { get; set; }

    public string Arguments { get; set; }

    public string HubName { get; set; }

    public string MethodName { get; set; }

    public bool IsIncoming { get; set; }

    [ForeignKey(nameof(AuditTicketId))]
    public AuditTicket AuditTicket { get; set; }
  }
}