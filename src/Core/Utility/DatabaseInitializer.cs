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

      this.InitializeUsers(ticket.TicketId);

      ticket.EndTime = DateTimeOffset.Now;
      this.DbContext.SaveChanges();

      this.Logger.LogDebug("Database initialized");
    }

    private void InitializeSecurityProfiles(long ticketId)
    {
      this.Logger.LogDebug("Initializing security profiles");

      this.Logger.LogDebug("Security profiles initialized");
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

        var nodeClosureMap = new NodeClosureMap()
        {
          AncestorId = node.Id,
          DescendantId = node.Id,
          PathLength = 0,
          CreatedTicketId = ticketId
        };
        this.DbContext.NodeClosureMaps.Add(nodeClosureMap);
        this.DbContext.SaveChanges();

        var user = new UserAccount()
        {
          Id = node.Id,
          UserName = username,
          Culture = this.DefaultCulture,
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