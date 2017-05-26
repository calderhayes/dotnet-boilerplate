namespace DotNetBoilerplate.Api.Hubs
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.SignalR;
  using Microsoft.Extensions.Logging;

  public abstract class BaseHub
    : Hub
  {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected virtual ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected IUserContext UserContext
    {
      get
      {
        return (IUserContext)this.Context.Request.HttpContext.Items["UserContext"];
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override Task OnConnected()
    {
      this.Logger.LogDebug("Connecting a user to a hub");
      return base.OnConnected();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stopCalled"></param>
    /// <returns></returns>
    public override Task OnDisconnected(bool stopCalled)
    {
      this.Logger.LogDebug("Disconnecting a user from the hub");
      return base.OnDisconnected(stopCalled);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override Task OnReconnected()
    {
      this.Logger.LogDebug("Reconnecting a user from the hub");
      return base.OnReconnected();
    }
  }
}