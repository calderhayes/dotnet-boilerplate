namespace DotNetBoilerplate.Data.Entity
{
  public interface IResponseLog
  {
    long AuditTicketId { get; set; }

    string ResponseBody { get; set; }
  }
}
