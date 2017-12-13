namespace DotNetBoilerplate.Auth
{
  using System.IO;
  using System.Security.Authentication;
  using System.Security.Cryptography.X509Certificates;
  using Microsoft.AspNetCore.Hosting;
  // using Microsoft.AspNetCore.Server.Kestrel.Https;
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
          // var httpsOptions =
          //   new HttpsConnectionFilterOptions();
          // httpsOptions.ClientCertificateMode = ClientCertificateMode.NoCertificate;
          // httpsOptions.CheckCertificateRevocation = false;
          // httpsOptions.ServerCertificate =
          //   new X509Certificate2("testCert.pfx", "testPassword");
          // httpsOptions.SslProtocols = SslProtocols.Tls;

          // options.ThreadCount = 4;
          // options.NoDelay = true;
          // options.UseHttps(httpsOptions);
          // options.UseConnectionLogging();
        })
        .UseConfiguration(config)
        .UseUrls("http://*:5050") // , "https://*:5051")
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>()
        .Build();

      host.Run();
    }
  }
}
