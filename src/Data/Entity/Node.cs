namespace DotNetBoilerplate.Data.Entity
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public class Node
    : INode
  {
    /// <summary>
    /// Primary Key
    /// </summary>
    /// <returns></returns>
    [Key]
    public long Id { get; set; }

    [Required]
    public Guid ExternalId { get; set; }

    [Required]
    public string Label { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public NodeType NodeType { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    [Required]
    public long ModifiedTicketId { get; set; }

    [ForeignKey(nameof(ModifiedTicketId))]
    public AuditTicket ModifiedAuditTicket { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Node>()
        .HasIndex(p => new { p.ExternalId })
        .IsUnique(true);
    }
  }
}