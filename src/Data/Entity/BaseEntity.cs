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
  public abstract class BaseEntity
    : IBaseEntity
  {
    public BaseEntity()
    {
    }

    public BaseEntity(BaseEntity entity)
    {
      this.Id = entity.Id;
      this.ExternalId = entity.ExternalId;
      this.IsDeleted = entity.IsDeleted;
      this.CreatedAuditTicket = entity.CreatedAuditTicket;
      this.CreatedTicketId = entity.CreatedTicketId;
      this.ModifiedAuditTicket = entity.ModifiedAuditTicket;
      this.ModifiedTicketId = entity.ModifiedTicketId;
    }

    /// <summary>
    /// Primary Key
    /// </summary>
    /// <returns></returns>
    [Key]
    public long Id { get; set; }

    [Required]
    public Guid ExternalId { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    [Required]
    public long ModifiedTicketId { get; set; }

    [ForeignKey(nameof(ModifiedTicketId))]
    public AuditTicket ModifiedAuditTicket { get; set; }

    public static void OnModelCreating<T>(ModelBuilder modelBuilder)
      where T : BaseEntity
    {
      modelBuilder.Entity<T>()
        .HasIndex(p => new { p.ExternalId })
        .IsUnique(true);
    }
  }
}