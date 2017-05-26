namespace DotNetBoilerplate.Core.Logic
{
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using DotNetBoilerplate.Core.Model;
  using Microsoft.Extensions.Logging;
  using Newtonsoft.Json;

  public class CultureProvider
    : ICultureProvider
  {
    private const string CultureToken = "{culture}";

    private const string TranslationJsonResourceName
      = "Core.i18next.i18next-{culture}.json";

    private readonly ILogger logger;

    private readonly TranslationCollection translations;

    public CultureProvider(
      IApplicationConfiguration config,
      ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger(nameof(CultureProvider));
      this.translations = new TranslationCollection(loggerFactory);
    }

    public Stream GetTranslationJsonFileStream(string culture)
    {
      var assembly = typeof(CultureProvider).GetTypeInfo().Assembly;
      var resourceName = TranslationJsonResourceName.Replace(CultureToken, culture);
      var resourceStream = assembly.GetManifestResourceStream(resourceName);

      if (resourceStream == null)
      {
        var availableResources = string.Join(", ", assembly.GetManifestResourceNames());
        throw new CoreException(
          $"Could not find the embedded resource: {resourceName}, available resources are {availableResources}");
      }

      return resourceStream;
    }

    public ManifestResourceInfo GetTranslationJsonInfo(string culture)
    {
      var assembly = typeof(CultureProvider).GetTypeInfo().Assembly;
      var resourceName = TranslationJsonResourceName.Replace(CultureToken, culture);
      var info = assembly.GetManifestResourceInfo(resourceName);

      return info;
    }

    public string GetServerTranslatedValue(string culture, string key)
    {
      if (!this.translations.IsCultureLoaded(culture))
      {
        this.LoadTranslations(culture);
        this.LoadTranslations(culture.Substring(0, 2));
      }

      return this.translations.GetTranslation(culture, key);
    }

    private void LoadTranslations(string culture)
    {
      using (var stream = this.GetTranslationJsonFileStream(culture))
      using (var reader = new StreamReader(stream))
      {
        string text = reader.ReadToEnd();
        this.translations.LoadTranslationsForCulture(
          culture, JsonConvert.DeserializeObject<Dictionary<string, string>>(text));
      }
    }
  }
}
