namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;

  public class PrincipalClosureMap
    : IPrincipalClosureMap
  {
    [Required]
    public long AncestorId { get; set; }

    [ForeignKey(nameof(AncestorId))]
    public Principal AncestorPrincipal { get; set; }

    [Required]
    public long DescendantId { get; set; }

    [ForeignKey(nameof(DescendantId))]
    public Principal DescendantPrincipal { get; set; }

    [Required]
    public PrincipalClosureMapDomain Domain { get; set; }

    [Required]
    public int PathLength { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<PrincipalClosureMap>()
        .HasKey(e => new { e.AncestorId, e.DescendantId });
    }
  }
}