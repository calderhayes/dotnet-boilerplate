namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  ///
  /// </summary>
  public class UserAuthenticationSource
    : IUserAuthenticationSource
  {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Required]
    public string AuthenticationSource { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Required]
    public string Subject { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Required]
    public long UserId { get; set; }

    [Required]
    public long CreatedTicketId { get; set; }

    [ForeignKey(nameof(CreatedTicketId))]
    public AuditTicket CreatedAuditTicket { get; set; }

    public long ModifiedTicketId { get; set; }

    [ForeignKey(nameof(ModifiedTicketId))]
    public AuditTicket ModifiedAuditTicket { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [ForeignKey(nameof(UserId))]
    public UserAccount User { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserAuthenticationSource>()
        .HasKey(e => new { e.AuthenticationSource, e.Subject, e.UserId });
    }
  }
}