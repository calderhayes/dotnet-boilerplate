namespace DotNetBoilerplate.Core.Logic
{
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
      return await this.NodeProvider.CreateNode(
        label, NodeType.User, utx.AuditTicketId);
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

    /// <summary>
    /// For the anonymous user
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> GetAnonymousToggles()
    {
      var baseList = GetBaseToggles();
      foreach (var elem in baseList)
      {
        // Enable everything but the login;
        elem.IsEnabled = false;
      }

      return baseList;
    }

    /// <summary>
    /// For the system user
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> GetSystemToggles()
    {
      var baseList = GetBaseToggles();
      foreach (var elem in baseList)
      {
        // Enable everything but the login;
        elem.IsEnabled = elem.ToggleType != SecurityProfileToggleType.CanLogin;
      }

      return baseList;
    }

    /// <summary>
    /// If toggles are disabled here, they are disabled everywhere
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> GetMasterToggles()
    {
      var map = new List<(SecurityProfileToggleType, bool)>()
      {
        (SecurityProfileToggleType.CanViewDashboard, true),
        (SecurityProfileToggleType.CanLogin, true),
        (SecurityProfileToggleType.SendLiveEmail, false)
      };

      return BuildToggleList(map);
    }

    /// <summary>
    /// For a generic user (for now)
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> GetDefaultToggles()
    {
      return GetBaseToggles();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> BuildToggleList(IList<(SecurityProfileToggleType, bool)> map)
    {
      return map.Select(m =>
      {
        var (type, enabled) = m;
        var t = GetBaseToggle(type);
        t.IsEnabled = enabled;
        return t;
      })
      .ToList();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static ISecurityProfileToggle GetBaseToggle(SecurityProfileToggleType type)
    {
      return GetBaseToggles()
        .Where(t => t.ToggleType == type)
        .Single();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private static IList<ISecurityProfileToggle> GetBaseToggles()
    {
      return new List<ISecurityProfileToggle>()
      {
        new SecurityProfileToggle()
        {
          ToggleType = SecurityProfileToggleType.CanViewDashboard,
          Category = SecurityProfileToggleCategory.Permission,
          IsEnabled = true,
          IsDynamic = true
        },
        new SecurityProfileToggle()
        {
          ToggleType = SecurityProfileToggleType.CanLogin,
          Category = SecurityProfileToggleCategory.Permission,
          IsEnabled = true,
          IsDynamic = true
        },
        new SecurityProfileToggle()
        {
          ToggleType = SecurityProfileToggleType.SendLiveEmail,
          Category = SecurityProfileToggleCategory.Operational,
          IsEnabled = true,
          IsDynamic = true
        }
      };
    }
  }
}