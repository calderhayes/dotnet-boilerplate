namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;

  /// <summary>
  /// Main toggle details
  /// </summary>
  public class SecurityProfileToggle
    : ISecurityProfileToggle
  {
    [Key]
    public SecurityProfileToggleType ToggleType { get; set; }

    [Required]
    public SecurityProfileToggleCategory Category { get; set; }

    /// <summary>
    /// Handles the global toggling
    /// </summary>
    /// <returns></returns>
    [Required]
    public bool IsEnabled { get; set; }

    [Required]
    public bool IsDynamic { get; set; }
  }
}