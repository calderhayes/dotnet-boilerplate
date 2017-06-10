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

      var principalProvider = new PrincipalProvider(
        dbContext,
        loggerFactory.CreateLogger<PrincipalProvider>());

      this.AccountProvider = new AccountProvider(
        dbContext,
        loggerFactory.CreateLogger<AccountProvider>(),
        principalProvider);
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

    public void Initialize()
    {
      this.Logger.LogDebug("Initializing database");

      var ticket = this.GetInitialTicket();

      this.InitializeUsers(ticket.TicketId);

      ticket.EndTime = DateTimeOffset.Now;
      this.DbContext.SaveChanges();

      this.Logger.LogDebug("Database initialized");
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
        var principal = new Principal()
        {
          ExternalId = Guid.NewGuid(),
          Label = username,
          PrincipalType = PrincipalType.User,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };
        this.DbContext.Principals.Add(principal);
        this.DbContext.SaveChanges();

        var principalClosureMap = new PrincipalClosureMap()
        {
          AncestorId = principal.Id,
          DescendantId = principal.Id,
          PathLength = 0,
          CreatedTicketId = ticketId
        };
        this.DbContext.PrincipalClosureMaps.Add(principalClosureMap);
        this.DbContext.SaveChanges();

        var user = new UserAccount()
        {
          Id = principal.Id,
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