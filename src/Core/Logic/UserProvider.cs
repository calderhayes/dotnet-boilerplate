namespace DotNetBoilerplate.Core.Logic
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  public class UserProvider
    : IUserProvider
  {
    public UserProvider(IDbContext dbContext, ILogger<UserProvider> logger)
    {
      this.DbContext = dbContext;
      this.Logger = logger;

      var eventId = 123;
      this.Logger.LogDebug(eventId, "blah");
    }

    private IDbContext DbContext { get; }

    private ILogger Logger { get; }

    public async Task<long> GetUserCount()
    {
      return await this.DbContext.UserAccounts.CountAsync();
    }
  }
}
