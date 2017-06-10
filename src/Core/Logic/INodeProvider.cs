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
      NodeType nodeType);

    Task<INode> CreateNode(
      string label,
      NodeType nodeType,
      long auditTicketId);

    Task AddChildNode(
      long ticketId,
      long parentNodeId,
      long childNodeId,
      NodeClosureMapDomain domain);
  }
}