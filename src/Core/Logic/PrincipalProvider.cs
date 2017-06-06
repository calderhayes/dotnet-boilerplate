namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
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

    public async Task<Principal> CreatePrincipal(
      IUserContext userContext,
      string label,
      PrincipalType principalType)
    {
      return await this.CreatePrincipal(
        label, principalType, userContext.AuditTicketId);
    }

    public async Task<Principal> CreatePrincipal(
      string label,
      PrincipalType principalType,
      long auditTicketId)
    {
      var principal = new Principal()
      {
        Label = label,
        PrincipalType = principalType,
        CreatedTicketId = auditTicketId,
        ModifiedTicketId = auditTicketId
      };
      this.DbContext.Principals.Add(principal);
      await this.DbContext.SaveChangesAsync();

      var principalClosureMap = new PrincipalClosureMap()
      {
        AncestorId = principal.Id,
        DescendantId = principal.Id,
        PathLength = 0,
        CreatedTicketId = auditTicketId
      };
      this.DbContext.PrincipalClosureMaps.Add(principalClosureMap);
      await this.DbContext.SaveChangesAsync();

      return principal;
    }
  }
}