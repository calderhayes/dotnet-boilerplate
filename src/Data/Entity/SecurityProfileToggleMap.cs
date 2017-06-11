namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public class SecurityProfileToggleMap
  {
    public long SecurityProfileId { get; set; }

    public SecurityProfileToggleType ToggleType { get; set; }

    /// <summary>
    /// Handles the local profile mapping
    /// </summary>
    /// <returns></returns>
    [Required]
    public bool IsEnabled { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<SecurityProfileToggleMap>()
        .HasKey(m => new { m.SecurityProfileId, m.ToggleType });
    }
  }
}