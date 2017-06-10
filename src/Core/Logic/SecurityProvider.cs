namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.Extensions.Logging;

  public class SecurityProvider
  {
    public SecurityProvider(
      IDbContext dbContext,
      ILogger<SecurityProvider> logger,
      INodeProvider nodeProvider)
    {
      this.Logger = logger;
      this.DbContext = dbContext;
      this.NodeProvider = nodeProvider;
    }

    private IDbContext DbContext { get; }

    private ILogger Logger { get; }

    private INodeProvider NodeProvider { get; }

    public Task<INode> CreateSecurityProfile(
      IUserContext utx, string label)
    {
      return this.NodeProvider.CreateNode(
        label, NodeType.SecurityProfile, utx.AuditTicketId);
    }
  }
}