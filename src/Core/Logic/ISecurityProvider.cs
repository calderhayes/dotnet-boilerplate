namespace DotNetBoilerplate.Core.Logic
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;

  public interface ISecurityProvider
  {
    Task<bool> IsToggleEnabled(
      long securityProfileId, SecurityProfileToggleType type);

    Task<IList<ISecurityProfileToggle>> GetProfileToggles(
      long securityProfileId);

    bool IsToggleEnabled(
      IUserContext userContext, SecurityProfileToggleType type);
  }
}