namespace DotNetBoilerplate.Api
{
  using System;
  using System.Linq;
  using System.Text;
  using DotNetBoilerplate.Api.Utility;
  using DotNetBoilerplate.Core.Constants;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Core.Logic.Email;
  using DotNetBoilerplate.Core.Model;
  using DotNetBoilerplate.Data;
  using Hangfire;
  using Hangfire.PostgreSql;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using Microsoft.IdentityModel.Tokens;
  using Newtonsoft.Json;
  using SimpleFileControl;

  public class Startup
  {
    private const string MainFileDirectoryName = "files";

    private const string ArchiveDirectoryName = "archive";

    public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      builder.AddEnvironmentVariables();

      this.LoggerFactory = loggerFactory;

      this.Configuration = builder.Build();

      this.CurrentEnvironment = env;

      this.Logger = loggerFactory.CreateLogger<Startup>();
    }

    private IHostingEnvironment CurrentEnvironment { get; }

    private ILoggerFactory LoggerFactory { get; }

    private ILogger Logger { get; }

    private IConfigurationRoot Configuration { get; set; }

    // This method gets called by the runtime.
    // We use this to add services to the container
    public void ConfigureServices(
      IServiceCollection services)
    {
      var config = new ApplicationConfiguration(this.Configuration);

      // http://stackoverflow.com/questions/40097229/when-i-develop-asp-net-core-mvc-which-service-should-i-use-addmvc-or-addmvccor
      var builder = services.AddMvcCore(o =>
      {
        o.Filters.Add(new GlobalExceptionFilter(this.LoggerFactory, config));
      });

      builder.AddApiExplorer();
      builder.AddAuthorization();

      builder.AddFormatterMappings();
      builder.AddCacheTagHelper();

      builder.AddDataAnnotations();

      builder.AddJsonFormatters(j =>
      {
        j.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      });

      builder.AddCors();

      services.AddHangfire(hconfig =>
      {
        var connectionString = this.Configuration.GetConnectionString("ApplicationDatabase");
        hconfig.UsePostgreSqlStorage(connectionString);
      });

      this.ConfigureAuthenticationMiddleware(services);
      this.ConfigureFileControl(services);
      this.ConfigureDataStoreServices(services);
      this.ConfigureLogicProviderServices(services);

      this.Logger.LogInformation("Services configured");
    }

    // This method is called at runtime
    // Use this method to configure the HTTP request pipeline
    // This is where middleware is assigned
    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory,
      PostgreSQLContext dbContext)
    {
      loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseMiddleware<QueryStringAuthHeaderMiddleware>();

      var config = new ApplicationConfiguration(this.Configuration);

      app.UseMiddleware<UserContextMiddleware>();

      if (config.LogWebApiRequests)
      {
        app.UseMiddleware<LogRequestMiddleware>();
      }

      if (config.LogWebApiResponses)
      {
        app.UseMiddleware<LogResponseMiddleware>();
      }

      app.UseCors(builder =>
      {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.WithOrigins(config.CorsAllowedOrigins.ToArray());
        // Need this for signalr
        builder.AllowCredentials();
      });

      app.UseAuthentication();
      app.UseMvc();
      app.UseWebSockets();

      app.UseHangfireDashboard();
      // hangfire server is available on URL/hangfire
      app.UseHangfireServer();

      // For testing
      BackgroundJob.Enqueue(() => System.Console.WriteLine("HANGFIRE TEST"));

      var databaseInitializer = new Core.Utility.DatabaseInitializer(
        dbContext, loggerFactory, config.DefaultCulture);
      databaseInitializer.Initialize();

      this.Logger.LogInformation("App configured");
    }

    private void ConfigureFileControl(IServiceCollection services)
    {
      var contents = this.CurrentEnvironment.ContentRootFileProvider.GetDirectoryContents(string.Empty);
      var doesFileDirectoryExist = contents
        .Where(c => c.IsDirectory && c.Name == MainFileDirectoryName)
        .Any();

      var mainRoot = System.IO.Path.Combine(this.CurrentEnvironment.ContentRootPath, MainFileDirectoryName);
      if (!doesFileDirectoryExist)
      {
        System.IO.Directory.CreateDirectory(mainRoot);
      }

      var doesArchiveDirectoryExist = contents
        .Where(c => c.IsDirectory && c.Name == ArchiveDirectoryName)
        .Any();

      var archiveRoot = System.IO.Path.Combine(this.CurrentEnvironment.ContentRootPath, ArchiveDirectoryName);
      if (!doesArchiveDirectoryExist)
      {
        System.IO.Directory.CreateDirectory(archiveRoot);
      }

      services.AddSingleton<IFileStoreCollection>(fs =>
      {
        var fileControl = new FileStoreCollection();
        var localDiskStore = new SimpleFileControl.Store.LocalDiskStore("localdisk", mainRoot, archiveRoot);
        fileControl.RegisterFileStore(localDiskStore);
        fileControl.DefaultFileStoreId = "localdisk";
        return fileControl;
      });
    }

    private void ConfigureDataStoreServices(IServiceCollection services)
    {
      // This will eventually be configuration driven (?)
      // We can switch between postgres and sqlite for dev work
      var connectionString = this.Configuration.GetConnectionString("ApplicationDatabase");

      services.AddDbContext<PostgreSQLContext>(options =>
      {
        // https://damienbod.com/2016/01/11/asp-net-5-with-postgresql-and-entity-framework-7/
        options.UseNpgsql(connectionString);
      });

      services.AddScoped<IDbContext>(provider => provider.GetService<PostgreSQLContext>());
    }

    private void ConfigureLogicProviderServices(IServiceCollection services)
    {
      services.AddSingleton<IApplicationConfiguration>(o => new ApplicationConfiguration(this.Configuration));
      services.AddSingleton<ICultureProvider, CultureProvider>();
      services.AddScoped<ISecurityProvider, SecurityProvider>();
      services.AddScoped<IEmailProviderFactory, EmailProviderFactory>();
      services.AddScoped<INodeProvider, NodeProvider>();
      services.AddScoped<IAccountProvider, AccountProvider>();
      services.AddScoped<IAuditProvider, AuditProvider>();
      services.AddScoped<IUserProvider, UserProvider>();
    }

    private void ConfigureAuthenticationMiddleware(IServiceCollection services)
    {
      var config = this.Configuration.GetSection("Authentication");
      var secret = Encoding.ASCII.GetBytes(config.GetValue<string>("Secret"));
      var secretKey = new SymmetricSecurityKey(secret);

      var options = new JwtBearerOptions()
      {
        Audience = config.GetValue<string>("Audience"),

        RequireHttpsMetadata = config.GetValue<bool>("RequireHttpsMetadata"),

        TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = secretKey,
          ValidIssuer = config.GetValue<string>("Issuer"),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true
        }
      };

      services.AddAuthentication(opts =>
      {
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(opts =>
      {
        opts.Audience = config.GetValue<string>("Audience");

        opts.RequireHttpsMetadata = config.GetValue<bool>("RequireHttpsMetadata");

        opts.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = secretKey,
          ValidIssuer = config.GetValue<string>("Issuer"),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true
        };
      });
    }
  }
}
