namespace DotNetBoilerplate.Data
{
  using System;
  using System.Data;
  using System.Threading;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data.Entity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.ChangeTracking;

  public interface IDbContext
  {
    DbSet<SecurityProfileToggleMap> SecurityProfileToggleMaps { get; set; }

    DbSet<SecurityProfileToggle> SecurityProfileToggles { get; set; }

    DbSet<SecurityProfile> SecurityProfiles { get; set; }

    DbSet<NodeClosureMap> NodeClosureMaps { get; }

    DbSet<Node> Nodes { get; }

    DbSet<UserAccount> UserAccounts { get; }

    DbSet<AuditTicket> AuditTickets { get; }

    DbSet<UserAuthenticationSource> UserAuthenticationSources { get; }

    DbSet<ResponseLog> ResponseLogs { get; set; }

    DbSet<RequestLog> RequestLogs { get; set; }

    DbSet<HubLog> HubLogs { get; set; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
      where TEntity : class;

    void AttachIfDoesNotExist<TEntity>(TEntity entity)
      where TEntity : class;

    EntityEntry Entry(object entity);

    int SaveChanges();

    Task<T> RunInTransaction<T>(Func<Task<T>> func);

    Task<T> RunInTransaction<T>(IsolationLevel isolationLevel, Func<Task<T>> func);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync();
  }
}
