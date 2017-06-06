namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;

  /// <summary>
  /// Defines a base principal object
  /// </summary>
  public class Principal
    : IPrincipal
  {
    /// <summary>
    /// Primary Key
    /// </summary>
    /// <returns></returns>
    [Key]
    public long Id { get; set; }

    [Required]
    public string Label { get; set; }

    [Required]
    public PrincipalType PrincipalType { get; set; }

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