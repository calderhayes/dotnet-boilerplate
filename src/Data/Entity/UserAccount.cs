namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;

  public class UserAccount
    : IUserAccount
  {
    [Key]
    public long UserId { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Culture { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    [Required]
    public long ModifiedTicketId { get; set; }

    [ForeignKey(nameof(ModifiedTicketId))]
    public AuditTicket ModifiedAuditTicket { get; set; }

  }
}
