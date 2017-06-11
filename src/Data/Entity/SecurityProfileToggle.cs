namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using DotNetBoilerplate.Data.Model.Lookup;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public class SecurityProfileToggle
  {
    // GLOBAL AND USER SCOPE?
    [Key]
    public SecurityProfileToggleType ToggleType { get; set; }

    [Required]
    public SecurityProfileToggleCategory Category { get; set; }

    /// <summary>
    /// Handles the global toggling
    /// </summary>
    /// <returns></returns>
    public bool IsEnabled { get; set; }

    public bool IsDynamic { get; set; }
  }
}