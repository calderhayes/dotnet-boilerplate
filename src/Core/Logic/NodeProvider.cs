namespace DotNetBoilerplate.Core.Logic
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  public class NodeProvider
    : INodeProvider
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public NodeProvider(
      IDbContext dbContext,
      ILogger<NodeProvider> logger)
    {
      this.Logger = logger;
      this.DbContext = dbContext;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IDbContext DbContext { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userContext"></param>
    /// <param name="label"></param>
    /// <param name="principalType"></param>
    /// <returns></returns>
    public async Task<INode> CreateNode(
      IUserContext userContext,
      string label,
      NodeType principalType)
    {
      return await this.CreateNode(
        label, principalType, userContext.AuditTicketId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="label"></param>
    /// <param name="principalType"></param>
    /// <param name="auditTicketId"></param>
    /// <returns></returns>
    public async Task<INode> CreateNode(
      string label,
      NodeType principalType,
      long auditTicketId)
    {
      var node = new Node()
      {
        ExternalId = Guid.NewGuid(),
        Label = label,
        NodeType = principalType,
        CreatedTicketId = auditTicketId,
        ModifiedTicketId = auditTicketId
      };
      this.DbContext.Nodes.Add(node);
      await this.DbContext.SaveChangesAsync();

      // Not sure if this is what we want
      var allDomains = new List<NodeClosureMapDomain>()
      {
        NodeClosureMapDomain.SecurityProfileAssignment
      };

      foreach (var domain in allDomains)
      {
        var nodeClosureMap = new NodeClosureMap()
        {
          AncestorId = node.Id,
          DescendantId = node.Id,
          PathLength = 0,
          Domain = domain,
          CreatedTicketId = auditTicketId
        };
        this.DbContext.NodeClosureMaps.Add(nodeClosureMap);
        await this.DbContext.SaveChangesAsync();
      }

      return node;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="parentPrincipalId"></param>
    /// <param name="childPrincipalId"></param>
    /// <param name="domain"></param>
    /// <returns></returns>
    public async Task AddChildNode(
      long ticketId,
      long parentPrincipalId,
      long childPrincipalId,
      NodeClosureMapDomain domain)
    {
      // We must have this adhere to a directed acyclic graph, otherwise it gets very very messy
      var violationExists = await this.DbContext.NodeClosureMaps
        .Where(m =>
          m.AncestorId == childPrincipalId
          && m.DescendantId == parentPrincipalId
          && m.Domain == domain)
        .AnyAsync();

      if (violationExists)
      {
        var msg = $"Attempting to add child principal {childPrincipalId.ToString()} ";
        msg += $"To parent principal {parentPrincipalId.ToString()} under domain {domain.ToString()} ";
        msg += "when the parent already exists as a child of the child principal, breaking the DAG property";
        throw new CoreException(msg);
      }

      var existingMapsToUpdate = await this.DbContext.NodeClosureMaps
        .Where(m => m.DescendantId == parentPrincipalId && m.Domain == domain)
        .Select(m => new
        {
          m.AncestorId,
          m.PathLength
        })
        .ToListAsync();

      var mapsToAdd = new List<NodeClosureMap>();
      foreach (var map in existingMapsToUpdate)
      {
        var newMap = new NodeClosureMap()
        {
          AncestorId = map.AncestorId,
          DescendantId = childPrincipalId,
          PathLength = map.PathLength + 1,
          Domain = domain,
          CreatedTicketId = ticketId
        };

        mapsToAdd.Add(newMap);
      }

      this.DbContext.NodeClosureMaps.AddRange(mapsToAdd);

      await this.DbContext.SaveChangesAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="principalId"></param>
    /// <returns></returns>
    public async Task<INode> GetPrincipal(long principalId)
    {
      return await this.DbContext.Nodes
        .Where(p => p.Id == principalId)
        .SingleAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="principalId"></param>
    /// <param name="domain"></param>
    /// <param name="includeTarget"></param>
    /// <returns></returns>
    public IQueryable<INode> GetAncestorsQuery(
      long principalId, NodeClosureMapDomain domain, bool includeTarget)
    {
      var query =
      from maps in this.DbContext.NodeClosureMaps
      join p in this.DbContext.Nodes on maps.AncestorId equals p.Id
      where maps.DescendantId == principalId && maps.Domain == domain && (includeTarget || maps.PathLength != 0)
      select p;

      return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="principalId"></param>
    /// <param name="domain"></param>
    /// <param name="includeTarget"></param>
    /// <returns></returns>
    public IQueryable<INode> GetDescendantsQuery(
      long principalId, NodeClosureMapDomain domain, bool includeTarget)
    {
      var query =
      from maps in this.DbContext.NodeClosureMaps
      join p in this.DbContext.Nodes on maps.DescendantId equals p.Id
      where maps.AncestorId == principalId && maps.Domain == domain && (includeTarget || maps.PathLength != 0)
      select p;

      return query;
    }
  }
}