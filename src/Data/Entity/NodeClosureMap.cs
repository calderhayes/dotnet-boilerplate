namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;

  public class NodeClosureMap
    : INodeClosureMap
  {
    [Required]
    public long AncestorId { get; set; }

    [ForeignKey(nameof(AncestorId))]
    public Node AncestorNode { get; set; }

    [Required]
    public long DescendantId { get; set; }

    [ForeignKey(nameof(DescendantId))]
    public Node DescendantNode { get; set; }

    [Required]
    public NodeClosureMapDomain Domain { get; set; }

    [Required]
    public int PathLength { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<NodeClosureMap>()
        .HasKey(e => new { e.AncestorId, e.DescendantId });
    }
  }
}