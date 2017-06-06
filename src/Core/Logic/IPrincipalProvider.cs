namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.Extensions.Logging;

  public interface IPrincipalProvider
  {
    Task<Principal> CreatePrincipal(
      IUserContext userContext,
      string label,
      PrincipalType principalType);

    Task<Principal> CreatePrincipal(
      string label,
      PrincipalType principalType,
      long auditTicketId);
  }
}