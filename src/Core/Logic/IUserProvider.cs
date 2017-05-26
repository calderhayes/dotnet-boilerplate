namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;

  public interface IUserProvider
  {
    Task<long> GetUserCount();
  }
}
