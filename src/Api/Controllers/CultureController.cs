namespace DotNetBoilerplate.Api.Controllers
{
  using System.IO;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Data;
  using Microsoft.AspNetCore.Mvc;

  [Route("api/[controller]")]
  public class CultureController
    : BaseController
  {
    private readonly IDbContext dbContext;

    private readonly ICultureProvider cultureProvider;

    public CultureController(
      IDbContext dbContext,
      ICultureProvider cultureProvider)
    {
      this.dbContext = dbContext;
      this.cultureProvider = cultureProvider;
    }

    [HttpGet]
    [Route("translationJson/{culture}")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> GetTranslationJson(string culture)
    {
      this.HttpContext.Response.ContentType = "application/json";
      using (var stream = this.cultureProvider.GetTranslationJsonFileStream(culture))
      using (var memoryStream = new MemoryStream())
      {
        await stream.CopyToAsync(memoryStream);
        var file = memoryStream.ToArray();
        var result = new FileContentResult(file, this.HttpContext.Response.ContentType);
        return result;
      }
    }
  }
}