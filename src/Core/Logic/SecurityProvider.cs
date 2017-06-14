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

      this.MasterToggles = GetMasterToggles();
    }

    private IDbContext DbContext { get; }

    private ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private INodeProvider NodeProvider { get; }

    /// <summary>
    /// Cached master toggles (should be runtime based right now)
    /// </summary>
    /// <returns></returns>
    private IList<ISecurityProfileToggle> MasterToggles { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userContext"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsToggleEnabled(
      IUserContext userContext, SecurityProfileToggleType type)
    {
      return this.IsToggleEnabled(userContext.SecurityToggles, type);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="securityProfileId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<bool> IsToggleEnabled(
      long securityProfileId, SecurityProfileToggleType type)
    {
      var toggles = await this.GetProfileToggles(securityProfileId);
      return this.IsToggleEnabled(toggles, type);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="toggles"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool IsToggleEnabled(IList<ISecurityProfileToggle> toggles, SecurityProfileToggleType type)
    {
      var masterToggle = this.MasterToggles
        .Where(t => t.ToggleType == type)
        .Single();

      if (!masterToggle.IsEnabled)
      {
        return false;
      }

      var toggle = toggles
        .Where(t => t.ToggleType == type)
        .Single();

      return toggle.IsEnabled;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="securityProfileId"></param>
    /// <returns></returns>
    public async Task<IList<ISecurityProfileToggle>> GetProfileToggles(
      long securityProfileId)
    {
      // Non-persistant implementation for now
      var profile = await this.DbContext.SecurityProfiles
        .Where(s => s.Id == securityProfileId)
        .SingleAsync();

      if (profile.IsSystem)
      {
        switch (profile.Label)
        {
          case Constants.SystemSecurityProfileLabel.System:
            return GetSystemToggles();
          case Constants.SystemSecurityProfileLabel.Default:
            return GetDefaultToggles();
          case Constants.SystemSecurityProfileLabel.Anonymous:
            return GetAnonymousToggles();
        }
      }

      throw new CoreException("Security Profile not found");
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

    private static IList<ISecurityProfileToggle> BuildToggleList(IList<(SecurityProfileToggleType, bool)> map)
    {
      return map.Select(m =>
      {
        var
        (type, enabled) = m;
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
    /// Defines the main toggles
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