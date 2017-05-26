namespace DotNetBoilerplate.Core.Logic
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data.Model;

  public interface IAccountProvider
  {
    Task<IUserAccount> GetUserByAuthSourceAndSub(string authSource, string sub);

    Task<IUserAccount> RegisterUser(
      long auditTicketId,
      string authSource,
      string sub,
      string preferredUsername,
      string email,
      string locale,
      IList<string> roles);

    Task<IUserAccount> GetAnonymousAccount();
  }
}
