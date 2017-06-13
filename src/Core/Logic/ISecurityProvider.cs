namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;

  public interface ISecurityProvider
  {
    Task<bool> IsToggleEnabled(
      long securityProfileId, SecurityProfileToggleType type);
  }
}