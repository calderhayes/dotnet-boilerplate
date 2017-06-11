namespace DotNetBoilerplate.Data.Model.Lookup
{
  /// <summary>
  /// https://martinfowler.com/articles/feature-toggles.html
  /// </summary>
  public enum SecurityProfileToggleCategory
    : int
  {
    Permission = 0,
    Release = 1,
    Experiment = 2,
    Operational = 3
  }
}