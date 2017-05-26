namespace DotNetBoilerplate.Api
{
  using System.IO;
  using Microsoft.AspNetCore.Hosting;
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
        .UseKestrel()
        .UseConfiguration(config)
        .UseUrls("http://*:5080")
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>()
        .Build();

      host.Run();
    }
  }
}
