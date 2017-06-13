namespace DotNetBoilerplate.Data.Model.Lookup
{
  public enum SecurityProfileToggleType
    : int
  {
    // Permission toggles
    CanViewDashboard = 0,
    CanLogin = 1

    // Release toggles
    // Experiment toggles
    // Operational toggles
    SendLiveEmail = 100,
  }
}