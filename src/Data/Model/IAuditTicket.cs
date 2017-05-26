namespace DotNetBoilerplate.Data.Model
{
  using System;

  public interface IAuditTicket
  {
    long TicketId { get; set; }

    long? UserId { get; set; }

    string RequestId { get; set; }

    string IpAddress { get; set; }

    string RequestMethod { get; set; }

    string UserAgent { get; set; }

    string DisplayUrl { get; set; }

    string SecurityTokenIssuer { get; set; }

    string SecurityTokenId { get; set; }

    DateTimeOffset StartTime { get; set; }

    DateTimeOffset? EndTime { get; set; }
  }
}
