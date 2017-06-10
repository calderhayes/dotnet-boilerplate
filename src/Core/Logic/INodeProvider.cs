namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.Extensions.Logging;

  public interface INodeProvider
  {
    Task<INode> CreateNode(
      IUserContext userContext,
      string label,
      NodeType principalType);

    Task<INode> CreateNode(
      string label,
      NodeType principalType,
      long auditTicketId);

    Task AddChildNode(
      long ticketId,
      long parentPrincipalId,
      long childPrincipalId,
      NodeClosureMapDomain domain);
  }
}