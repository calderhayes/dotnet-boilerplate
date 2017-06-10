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

  public class PrincipalProvider
    : IPrincipalProvider
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public PrincipalProvider(
      IDbContext dbContext,
      ILogger<PrincipalProvider> logger)
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
    public async Task<Principal> CreatePrincipal(
      IUserContext userContext,
      string label,
      PrincipalType principalType)
    {
      return await this.CreatePrincipal(
        label, principalType, userContext.AuditTicketId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="label"></param>
    /// <param name="principalType"></param>
    /// <param name="auditTicketId"></param>
    /// <returns></returns>
    public async Task<Principal> CreatePrincipal(
      string label,
      PrincipalType principalType,
      long auditTicketId)
    {
      var principal = new Principal()
      {
        ExternalId = Guid.NewGuid(),
        Label = label,
        PrincipalType = principalType,
        CreatedTicketId = auditTicketId,
        ModifiedTicketId = auditTicketId
      };
      this.DbContext.Principals.Add(principal);
      await this.DbContext.SaveChangesAsync();

      var allDomains = new List<PrincipalClosureMapDomain>()
      {
        PrincipalClosureMapDomain.SecurityProfile
      };

      foreach (var domain in allDomains)
      {
        var principalClosureMap = new PrincipalClosureMap()
        {
          AncestorId = principal.Id,
          DescendantId = principal.Id,
          PathLength = 0,
          Domain = domain,
          CreatedTicketId = auditTicketId
        };
        this.DbContext.PrincipalClosureMaps.Add(principalClosureMap);
        await this.DbContext.SaveChangesAsync();
      }

      return principal;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="parentPrincipalId"></param>
    /// <param name="childPrincipalId"></param>
    /// <param name="domain"></param>
    /// <returns></returns>
    public async Task AddChildPrincipal(
      long ticketId,
      long parentPrincipalId,
      long childPrincipalId,
      PrincipalClosureMapDomain domain)
    {
      // We must have this adhere to a directed acyclic graph, otherwise it gets very very messy
      var violationExists = await this.DbContext.PrincipalClosureMaps
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

      var existingMapsToUpdate = await this.DbContext.PrincipalClosureMaps
        .Where(m => m.DescendantId == parentPrincipalId && m.Domain == domain)
        .Select(m => new
        {
          m.AncestorId,
          m.PathLength
        })
        .ToListAsync();

      var mapsToAdd = new List<PrincipalClosureMap>();
      foreach (var map in existingMapsToUpdate)
      {
        var newMap = new PrincipalClosureMap()
        {
          AncestorId = map.AncestorId,
          DescendantId = childPrincipalId,
          PathLength = map.PathLength + 1,
          Domain = domain,
          CreatedTicketId = ticketId
        };

        mapsToAdd.Add(newMap);
      }

      this.DbContext.PrincipalClosureMaps.AddRange(mapsToAdd);

      await this.DbContext.SaveChangesAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="principalId"></param>
    /// <returns></returns>
    public async Task<Principal> GetPrincipal(long principalId)
    {
      return await this.DbContext.Principals
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
    public IQueryable<IPrincipal> GetAncestorsQuery(
      long principalId, PrincipalClosureMapDomain domain, bool includeTarget)
    {
      var query =
      from maps in this.DbContext.PrincipalClosureMaps
      join p in this.DbContext.Principals on maps.AncestorId equals p.Id
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
    public IQueryable<IPrincipal> GetDescendantsQuery(
      long principalId, PrincipalClosureMapDomain domain, bool includeTarget)
    {
      var query =
      from maps in this.DbContext.PrincipalClosureMaps
      join p in this.DbContext.Principals on maps.DescendantId equals p.Id
      where maps.AncestorId == principalId && maps.Domain == domain && (includeTarget || maps.PathLength != 0)
      select p;

      return query;
    }
  }
}