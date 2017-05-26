namespace DotNetBoilerplate.Data.Model
{
  using System.ComponentModel.DataAnnotations;

  public interface IHubLog
  {
    [Key]
    long HubLogId { get; set; }

    long? AuditTicketId { get; set; }

    string Arguments { get; set; }

    string HubName { get; set; }

    string MethodName { get; set; }

    bool IsIncoming { get; set; }
  }
}