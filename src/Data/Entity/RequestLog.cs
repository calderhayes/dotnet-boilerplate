namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class RequestLog
    : IRequestLog
  {
    [Key]
    public long AuditTicketId { get; set; }

    public string RequestBody { get; set; }

    [ForeignKey(nameof(AuditTicketId))]
    public AuditTicket AuditTicket { get; set; }
  }
}
