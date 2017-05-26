namespace DotNetBoilerplate.Auth.Store
{
  using System.Text;
  using System.Threading.Tasks;
  using IdentityServer4.Stores;
  using Microsoft.IdentityModel.Tokens;

  internal class SigningCredentialStore
    : ISigningCredentialStore
  {
    public SigningCredentialStore(string secretKey)
    {
      var secret = Encoding.ASCII.GetBytes(secretKey);
      this.SecretKey = new SymmetricSecurityKey(secret);
      this.SigningCredentials = new SigningCredentials(this.SecretKey, SecurityAlgorithms.HmacSha512);
    }

    private SymmetricSecurityKey SecretKey { get; }

    private SigningCredentials SigningCredentials { get; }

    public Task<SigningCredentials> GetSigningCredentialsAsync()
    {
      return Task.FromResult(this.SigningCredentials);
    }
  }
}
