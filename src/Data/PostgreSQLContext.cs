namespace DotNetBoilerplate.Data
{
  using System;
  using System.Data;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data.Entity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  public class PostgreSQLContext
    : DbContext, IDbContext
  {
    public PostgreSQLContext(DbContextOptions options)
      : base(options)
    {
    }

    public DbSet<UserAccount> UserAccounts { get; set; }

    public DbSet<AuditTicket> AuditTickets { get; set; }

    public DbSet<UserAuthenticationSource> UserAuthenticationSources { get; set; }

    public DbSet<ResponseLog> ResponseLogs { get; set; }

    public DbSet<RequestLog> RequestLogs { get; set; }

    public DbSet<HubLog> HubLogs { get; set; }

    private IsolationLevel LastIsolationLevel { get; set; }

    public Task<int> SaveChangesAsync()
    {
      return this.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<T> RunInTransaction<T>(Func<Task<T>> func)
    {
      return await this.RunInTransaction(IsolationLevel.ReadCommitted, func);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="isolationLevel"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<T> RunInTransaction<T>(IsolationLevel isolationLevel, Func<Task<T>> func)
    {
      var isLocalTransaction = this.Database.CurrentTransaction == null;

      if (!isLocalTransaction && this.LastIsolationLevel != isolationLevel)
      {
        // This could be a problem
      }

      this.LastIsolationLevel = isolationLevel;

      using (var trx = this.Database.CurrentTransaction ?? await this.Database.BeginTransactionAsync(isolationLevel))
      {
        try
        {
          var result = await func();

          if (isLocalTransaction)
          {
            trx.Commit();
          }

          return result;
        }
        catch (Exception)
        {
          if (isLocalTransaction)
          {
            trx.Rollback();
          }

          throw;
        }
        finally
        {
          if (isLocalTransaction)
          {
            trx.Dispose();
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public bool Exists<TEntity>(TEntity entity)
      where TEntity : class
    {
        return this.Set<TEntity>().Local.Any(e => e == entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    public void AttachIfDoesNotExist<TEntity>(TEntity entity)
      where TEntity : class
    {
        if (!this.Exists<TEntity>(entity))
        {
          this.Set<TEntity>().Attach(entity);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      UserAuthenticationSource.OnModelCreating(builder);

      builder.Entity<UserAccount>()
        .HasIndex(u => u.UserName)
        .IsUnique();
    }
  }
}