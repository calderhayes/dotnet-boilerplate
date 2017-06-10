namespace DotNetBoilerplate.Core.Logic
{
  using System.Linq;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  public class SecurityProvider
    : ISecurityProvider
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

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private INodeProvider NodeProvider { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="utx"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task<INode> CreateSecurityProfile(
      IUserContext utx, string label)
    {
      // Manually enforce unique labels, shouldn't be a big deal for this purpose
      var exists = await this.DbContext.Nodes
        .Where(n => n.Label == label && n.NodeType == NodeType.SecurityProfile && !n.IsDeleted)
        .AnyAsync();

      if (exists)
      {
        throw new CoreException("Creating a security profile with a label that already exists");
      }

      return await this.NodeProvider.CreateNode(
        label, NodeType.SecurityProfile, utx.AuditTicketId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task<INode> GetSecurityProfileIfExists(string label)
    {
      return await this.DbContext.Nodes
        .Where(n => n.Label == label && !n.IsDeleted)
        .SingleOrDefaultAsync();
    }
  }
}