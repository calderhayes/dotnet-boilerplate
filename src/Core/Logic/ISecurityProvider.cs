namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;

  public interface ISecurityProvider
  {
    Task<INode> CreateSecurityProfile(
      IUserContext utx, string label);

    Task<INode> GetSecurityProfileIfExists(string label);
  }
}