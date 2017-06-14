namespace DotNetBoilerplate.Core.Logic.Email
{
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data.Model;

  public interface IEmailProviderFactory
  {
    IEmailProvider Create(IUserContext userContext);

    Task<IEmailProvider> Create(long securityProfileId);
  }
}