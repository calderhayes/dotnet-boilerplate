namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.EntityFrameworkCore;

  public class UserAccount
    : IUserAccount, INode
  {
    [Key]
    public long Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Culture { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(Id))]
    public Node Node { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    [Required]
    public long ModifiedTicketId { get; set; }

    [ForeignKey(nameof(ModifiedTicketId))]
    public AuditTicket ModifiedAuditTicket { get; set; }
  }
}
