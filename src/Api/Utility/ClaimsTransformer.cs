namespace DotNetBoilerplate.Api.Utility
{
  using System.Security.Claims;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authentication;

  // http://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity
  public class ClaimsTransformer
    : IClaimsTransformer
  {
    public Task<ClaimsPrincipal> TransformAsync(ClaimsTransformationContext context)
    {
      // Placeholder
      return Task.FromResult(context.Principal);
    }
  }
}