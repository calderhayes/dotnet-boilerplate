namespace DotNetBoilerplate.Core.Utility
{
  using System;
  using System.Linq;
  using DotNetBoilerplate.Core.Logic;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Constants;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.Extensions.Logging;

  public class DatabaseInitializer
  {
    public DatabaseInitializer(
      IDbContext dbContext,
      ILoggerFactory loggerFactory,
      string defaultCulture)
    {
      this.DbContext = dbContext;
      this.Logger = loggerFactory.CreateLogger<DatabaseInitializer>();
      this.DefaultCulture = defaultCulture;

      var nodeProvider = new NodeProvider(
        dbContext,
        loggerFactory.CreateLogger<NodeProvider>());

      this.AccountProvider = new AccountProvider(
        dbContext,
        loggerFactory.CreateLogger<AccountProvider>(),
        nodeProvider);

      this.SecurityProvider = new SecurityProvider(
        dbContext,
        loggerFactory.CreateLogger<SecurityProvider>(),
        nodeProvider);
    }

    public DatabaseInitializer(
      IDbContext dbContext,
      ILoggerFactory loggerFactory,
      string defaultCulture,
      IAccountProvider accountProvider)
    {
      this.DbContext = dbContext;
      this.Logger = loggerFactory.CreateLogger<DatabaseInitializer>();
      this.DefaultCulture = defaultCulture;
      this.AccountProvider = accountProvider;
    }

    private IDbContext DbContext { get; }

    private ILogger Logger { get; }

    private string DefaultCulture { get; }

    private IAccountProvider AccountProvider { get; }

    private ISecurityProvider SecurityProvider { get; }

    public void Initialize()
    {
      this.Logger.LogDebug("Initializing database");

      var ticket = this.GetInitialTicket();

      this.InitializeDatabasePermissions(ticket.TicketId);
      this.InitializeSecurityProfiles(ticket.TicketId);
      this.InitializeUsers(ticket.TicketId);

      ticket.EndTime = DateTimeOffset.Now;
      this.DbContext.SaveChanges();

      this.Logger.LogDebug("Database initialized");
    }

    private void InitializeDatabasePermissions(long ticketId)
    {
      // Non persisted in DB for now
    }

    private void InitializeSecurityProfiles(long ticketId)
    {
      this.Logger.LogDebug("Initializing security profiles");

      this.AddSystemSecurityProfileIfDoesNotExist(
        ticketId, Constants.SystemSecurityProfileLabel.System);
      this.AddSystemSecurityProfileIfDoesNotExist(
        ticketId, Constants.SystemSecurityProfileLabel.Anonymous);
      this.AddSystemSecurityProfileIfDoesNotExist(
        ticketId, Constants.SystemSecurityProfileLabel.Default);

      this.Logger.LogDebug("Security profiles initialized");
    }

    private void AddSystemSecurityProfileIfDoesNotExist(
      long ticketId, string label)
    {
      var exists = this.DbContext.SecurityProfiles
        .Where(p => p.IsSystem && p.Label == label)
        .Any();

      if (!exists)
      {
        var profile = new SecurityProfile()
        {
          ExternalId = Guid.NewGuid(),
          Label = label,
          IsDeleted = false,
          IsSystem = true,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };

        this.DbContext.SecurityProfiles.Add(profile);
        this.DbContext.SaveChanges();
      }
    }

    private void InitializeUsers(long ticketId)
    {
      this.Logger.LogDebug("Initializing users");

      this.AddUserIfDoesNotExist(ticketId, SystemUsername.System);
      this.AddUserIfDoesNotExist(ticketId, SystemUsername.Anonymous);

      this.Logger.LogDebug("Users initialized");
    }

    private void AddUserIfDoesNotExist(
      long ticketId, string username)
    {
      var userExists = this.DbContext.UserAccounts
        .Where(u => u.UserName == username)
        .Any();

      if (!userExists)
      {
        var securityProfileLabel = Constants.SystemSecurityProfileLabel.Default;
        if (username == SystemUsername.Anonymous)
        {
          securityProfileLabel = Constants.SystemSecurityProfileLabel.Anonymous;
        }
        else if (username == SystemUsername.System)
        {
          securityProfileLabel = Constants.SystemSecurityProfileLabel.System;
        }

        var profile = this.DbContext.SecurityProfiles
          .Where(p => p.IsSystem && p.Label == securityProfileLabel)
          .Single();

        var node = new Node()
        {
          ExternalId = Guid.NewGuid(),
          Label = username,
          NodeType = NodeType.User,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };
        this.DbContext.Nodes.Add(node);
        this.DbContext.SaveChanges();

        var user = new UserAccount()
        {
          Id = node.Id,
          UserName = username,
          Culture = this.DefaultCulture,
          SecurityProfileId = profile.Id,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };
        this.DbContext.UserAccounts.Add(user);
        this.DbContext.SaveChanges();
      }
    }

    private AuditTicket GetInitialTicket()
    {
      long? userId = null;
      if (this.DbContext.UserAccounts.Any())
      {
        userId = this.DbContext.UserAccounts
          .Where(u => u.UserName == SystemUsername.System)
          .Select(u => u.Id)
          .Single();
      }
      else
      {
        this.Logger.LogInformation("No users found, creating userless ticket");
      }

      var ticket = new AuditTicket()
      {
        UserId = userId,
        IpAddress = "DatabaseInitializer",
        SecurityTokenIssuer = "DatabaseInitializer",
        SecurityTokenId = "DatabaseInitializer",
        StartTime = DateTimeOffset.Now
      };

      this.DbContext.AuditTickets.Add(ticket);
      this.DbContext.SaveChanges();

      return ticket;
    }
  }
}