namespace DotNetBoilerplate.Auth
{
  using System.IO;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Server.Kestrel.Https;
  using Microsoft.Extensions.Configuration;

  public class Program
  {
    public static void Main(string[] args)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json", optional: true)
        .Build();

      var host = new WebHostBuilder()
        .UseKestrel(options =>
        {
          // options.ThreadCount = 4;
          options.NoDelay = true;
          options.UseHttps("testCert.pfx", "testPassword");
          // options.UseConnectionLogging();
        })
        .UseConfiguration(config)
        .UseUrls("http://*:5050", "https://*:5051")
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>()
        .Build();

      host.Run();
    }
  }
}
