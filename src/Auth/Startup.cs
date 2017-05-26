namespace DotNetBoilerplate.Auth
{
  using DotNetBoilerplate.Auth.OAuthEntity;
  using IdentityServer4.Services;
  using IdentityServer4.Stores;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Cors.Infrastructure;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;

  public class Startup
  {
    private readonly ILoggerFactory loggerFactory;

    public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      builder.AddEnvironmentVariables();

      this.loggerFactory = loggerFactory;

      this.Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var authConfig = this.Configuration.GetSection("Authentication");

      var logger = this.loggerFactory.CreateLogger<DefaultCorsPolicyService>();
      var cors = new DefaultCorsPolicyService(logger)
      {
          AllowedOrigins = { "http://localhost:8080" },
      };
      cors.AllowAll = true;
      services.AddSingleton<ICorsPolicyService>(cors);

      services
        .AddSingleton<ISigningCredentialStore>(options =>
       {
         return new Store.SigningCredentialStore(authConfig.GetValue<string>("Secret"));
       })
        .AddIdentityServer()
        .AddInMemoryClients(ClientFactory.Get(this.Configuration))
        .AddInMemoryIdentityResources(ResourceFactory.GetIdentityResources())
        .AddInMemoryApiResources(ResourceFactory.GetApiResources(this.Configuration))
        .AddTestUsers(UserFactory.Get());

      services.AddMvcCore();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseIdentityServer();
      app.UseMvc();
    }
  }
}
