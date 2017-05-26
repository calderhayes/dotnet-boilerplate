namespace DotNetBoilerplate.Auth.OAuthEntity
{
  using System.Collections.Generic;
  using System.Security.Claims;
  using IdentityModel;
  using IdentityServer4.Test;

  internal static class UserFactory
  {
    public static List<TestUser> Get()
    {
      var email = "john.doe@fake.com";

      return new List<TestUser>()
      {
        new TestUser()
        {
          SubjectId = "SUBJECTID",
          Username = email,
          Password = "password123",
          Claims = new List<Claim>()
          {
            new Claim(JwtClaimTypes.Email, email),
            new Claim(JwtClaimTypes.Role, "admin"),
            new Claim(JwtClaimTypes.PreferredUserName, email),
            new Claim(JwtClaimTypes.Locale, "en-CA"),
            new Claim(JwtClaimTypes.Subject, "thesubjectvalue")
          }
        }
      };
    }
  }
}
