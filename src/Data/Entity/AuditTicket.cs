namespace DotNetBoilerplate.Data.Entity
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using DotNetBoilerplate.Data.Model;

  public class AuditTicket
    : IAuditTicket
  {
    [Key]
    public long TicketId { get; set; }

    public long? UserId { get; set; }

    public string RequestId { get; set; }

    public string IpAddress { get; set; }

    public string RequestMethod { get; set; }

    public string UserAgent { get; set; }

    public string DisplayUrl { get; set; }

    public string SecurityTokenIssuer { get; set; }

    public string SecurityTokenId { get; set; }

    [Required]
    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }
  }
}
