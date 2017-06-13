namespace DotNetBoilerplate.Data.Entity
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public class SecurityProfile
    : BaseEntity
  {
    [Required]
    public string Label { get; set; }

    [IsRequired]
    public bool IsSystem { get; set; }
  }
}