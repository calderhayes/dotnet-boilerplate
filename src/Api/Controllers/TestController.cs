namespace DotNetBoilerplate.Api.Controllers
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Logic.Email;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.File;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  ///
  /// </summary>
  [Route("api/[controller]")]
  public class TestController
    : BaseController
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="userProvider"></param>
    /// <param name="dbContext"></param>
    /// <param name="cultureProvider"></param>
    /// <param name="fileControl"></param>
    /// <param name="emailProviderFactory"></param>
    public TestController(
      IUserProvider userProvider,
      IDbContext dbContext,
      ICultureProvider cultureProvider,
      IFileControl fileControl,
      IEmailProviderFactory emailProviderFactory)
    {
      this.UserProvider = userProvider;
      this.DbContext = dbContext;
      this.CultureProvider = cultureProvider;
      this.FileControl = fileControl;
      this.EmailProviderFactory = emailProviderFactory;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IEmailProviderFactory EmailProviderFactory { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IDbContext DbContext { get; }

    private IFileControl FileControl { get; }

    private ICultureProvider CultureProvider { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IUserProvider UserProvider { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Route("test")]
    [HttpGet]
    public async Task<IActionResult> Test()
    {
      return await this.DbContext.RunInTransaction(async () =>
      {
        var databaseTest = false;
        try
        {
          databaseTest = (await this.UserProvider.GetUserCount()) != -213;
        }
        catch
        {
          databaseTest = false;
        }

        return this.Ok(new
        {
          Success = true,
          Timestamp = DateTimeOffset.Now,
          Database = databaseTest,
          IsAnonymous = this.UserContext.IsAnonymous,
          Culture = this.UserContext.UserAccount.Culture,
          TranslationTest = this.CultureProvider.GetServerTranslatedValue(
            this.UserContext.UserAccount.Culture, "key1")
        });
      });

    }

    [HttpGet]
    [Route("translationTest")]
    public async Task<IActionResult> TranslationTest()
    {
      await Task.FromResult(0);
      var frenchHelloWorld = this.CultureProvider.GetServerTranslatedValue(
        "fr-CA", "helloWorld");

      var englishHelloWorld = this.CultureProvider.GetServerTranslatedValue(
        "en", "helloWorld");

      var canadianDateFormat = this.CultureProvider.GetServerTranslatedValue(
        "en-CA", "defaultDateFormat");

      var americanDateFormat = this.CultureProvider.GetServerTranslatedValue(
        "en-US", "defaultDateFormat");

      return this.Ok(new
      {
        FrenchHelloWorld = frenchHelloWorld,
        EnglishHelloWorld = englishHelloWorld,
        CanadianDataFormat = canadianDateFormat,
        AmericantDateFormat = americanDateFormat
      });
    }

    [HttpGet]
    [Route("fileTest")]
    public IActionResult FileTest()
    {
      var tempFile = this.FileControl.DefaultFileStore.CreateTempFile();
      var copied = this.FileControl.DefaultFileStore.CopyFile(tempFile.FileId);
      var archived = this.FileControl.DefaultFileStore.DeleteFile(copied.FileId, true);
      return this.Ok(new
      {
        TempFile = tempFile,
        CopiedFile = copied,
        ArchivedFile = archived
      });
    }

    [HttpGet]
    [Route("emailTest")]
    public async Task<IActionResult> EmailTest()
    {
      var provider = this.EmailProviderFactory.Create(this.UserContext);
      await provider.SendTestEmail();
      return this.Ok();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Route("testprotected")]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> TestProtected()
    {
      var databaseTest = false;
      try
      {
        databaseTest = (await this.UserProvider.GetUserCount()) != -213;
      }
      catch
      {
        databaseTest = false;
      }

      var claims = this.User.Claims
        .Select(c => new
        {
          Name = c.Type,
          Value = c.Value
        })
        .ToList();

      // var firstClaim = claims.First();
      var remoteIpAddress = Utility.RequestFunctions.GetRequestIP(this.HttpContext, true);

      return this.Ok(new
      {
        Success = true,
        UserName = this.UserContext.UserAccount.UserName,
        Timestamp = DateTimeOffset.Now,
        Database = databaseTest,
        RemoteIpAddress = remoteIpAddress.ToString(),
        Name = this.User.Identity.Name,
        Culture = this.UserContext.UserAccount.Culture,
        Claims = claims
      });
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [Route("testexception")]
    [HttpGet]
    public IActionResult ExceptionTest()
    {
      throw new ApiException("This is a test");
    }
  }
}
