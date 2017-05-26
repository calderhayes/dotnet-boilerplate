namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class ResponseLog
    : IResponseLog
  {
    [Key]
    public long AuditTicketId { get; set; }

    public string ResponseBody { get; set; }

    [ForeignKey(nameof(AuditTicketId))]
    public AuditTicket AuditTicket { get; set; }
  }
}
