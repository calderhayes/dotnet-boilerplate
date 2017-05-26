namespace DotNetBoilerplate.Data.Entity
{
  public interface IRequestLog
  {
    long AuditTicketId { get; set; }

    string RequestBody { get; set; }
  }
}
