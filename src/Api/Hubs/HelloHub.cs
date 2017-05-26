namespace DotNetBoilerplate.Api.Hubs
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data;
  using Microsoft.AspNetCore.SignalR;
  using Microsoft.Extensions.Logging;

  public class HelloHub
    : BaseHub
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public HelloHub(ILogger<HelloHub> logger, IDbContext dbContext)
    {
      this.Logger = logger;
      this.DbContext = dbContext;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected IDbContext DbContext { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected override ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    public void SayHello()
    {
      this.DbContext.RunInTransaction(async () =>
      {
        await Task.FromResult(0);
        this.Clients.Others.SomeoneSaidHi(this.UserContext.UserAccount.UserName);
        this.Clients.Caller.YouSaidHi(this.UserContext.UserAccount.UserName);
        return true;
      });
    }

    [Authorize]
    public void SayHiAuthorized()
    {
      this.Clients.Others.SomeoneSaidHi(this.UserContext.UserAccount.UserName);
      this.Clients.Caller.YouSaidHi(this.UserContext.UserAccount.UserName);
    }

    public void TestException()
    {
      throw new ApiException("This is a test");
    }
  }
}