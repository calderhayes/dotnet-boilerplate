namespace DotNetBoilerplate.Data.Model
{
  using DotNetBoilerplate.Data.Model.Lookup;

  public interface ISecurityProfileToggle
  {
    SecurityProfileToggleType ToggleType { get; set; }
    SecurityProfileToggleCategory Category { get; set; }
    bool IsEnabled { get; set; }
    bool IsDynamic { get; set; }
  }
}