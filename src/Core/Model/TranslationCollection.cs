namespace DotNetBoilerplate.Core.Model
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Extensions.Logging;

  public class TranslationCollection
  {
    private readonly Dictionary<string, Dictionary<string, string>> translations;

    private readonly ILogger logger;

    public TranslationCollection(ILoggerFactory loggerFactory)
    {
      this.translations = new Dictionary<string, Dictionary<string, string>>();
      this.logger = loggerFactory.CreateLogger(nameof(TranslationCollection));
    }

    public void LoadTranslationsForCulture(
      string culture,
      Dictionary<string, string> translations)
    {
      if (string.IsNullOrEmpty(culture))
      {
        throw new ArgumentNullException(nameof(culture));
      }

      this.translations[culture] = translations;
    }

    public string GetTranslation(string culture, string key)
    {
      if (string.IsNullOrEmpty(culture))
      {
        throw new ArgumentNullException(nameof(culture));
      }

      if (string.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException(nameof(key));
      }

      var language = culture.Substring(0, 2);

      if (!this.translations.ContainsKey(culture)
        || !this.translations[culture].ContainsKey(key))
      {
        if (!this.translations.ContainsKey(language))
        {
          this.logger.LogWarning("Attempting to translate on an unloaded culture: {0}", culture);
          return key;
        }
        else
        {
          if (!this.translations[language].ContainsKey(key))
          {
            this.logger.LogWarning("Missing translation on culture {0} and key {1}", culture, key);
            return key;
          }
          else
          {
            return this.translations[language][key];
          }
        }
      }
      else
      {
        return this.translations[culture][key];
      }
    }

    public bool IsCultureLoaded(string culture)
    {
      if (string.IsNullOrEmpty(culture))
      {
        throw new ArgumentNullException(nameof(culture));
      }

      return this.translations.ContainsKey(culture);
    }
  }
}
