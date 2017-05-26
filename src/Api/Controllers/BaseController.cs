namespace DotNetBoilerplate.Api.Controllers
{
  using DotNetBoilerplate.Core.Model;
  using Microsoft.AspNetCore.Mvc;

  public abstract class BaseController
    : Controller
  {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected IUserContext UserContext
    {
      get
      {
        return (IUserContext)this.HttpContext.Items["UserContext"];
      }
    }
  }
}