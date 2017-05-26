namespace DotNetBoilerplate.Core.Logic
{
  using System.IO;
  using System.Reflection;

  public interface ICultureProvider
  {
    Stream GetTranslationJsonFileStream(string culture);

    ManifestResourceInfo GetTranslationJsonInfo(string culture);

    string GetServerTranslatedValue(string culture, string key);
  }
}
