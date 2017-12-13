namespace DotNetBoilerplate.Data.PostgreSQL
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Infrastructure;
  using DotNetBoilerplate.Data;
  using Microsoft.Extensions.Logging;
  using Microsoft.EntityFrameworkCore.Design;

    public class PostgreSQLContextFactory
    : IDesignTimeDbContextFactory<PostgreSQLContext>
  {

    public PostgreSQLContext CreateDbContext(string[] args)
    {
      return Create(new DbContextFactoryOptions());
    }

    public PostgreSQLContext Create(DbContextFactoryOptions options)
    {
      // var connectionString = this.Configuration.GetConnectionString("ApplicationDatabase");
      var connectionString = "Host=localhost;Database=app;Username=postgres;Port=5432;Pooling=true;Password=password;";
      var optionsBuilder = new DbContextOptionsBuilder<PostgreSQLContext>();
      // https://damienbod.com/2016/01/11/asp-net-5-with-postgresql-and-entity-framework-7/
      optionsBuilder.UseNpgsql(connectionString, b =>
      {
        b.MigrationsAssembly("Data.PostgreSQL");
      });

      return new PostgreSQLContext(optionsBuilder.Options);
    }
  }
}