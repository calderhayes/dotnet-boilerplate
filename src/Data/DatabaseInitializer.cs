namespace DotNetBoilerplate.Data
{
  using System;
  using System.Linq;
  using DotNetBoilerplate.Data.Constants;
  using DotNetBoilerplate.Data.Entity;
  using Microsoft.Extensions.Logging;

  public class DatabaseInitializer
  {
    public static void Initialize(
      IDbContext dbContext,
      ILoggerFactory loggerFactory,
      string defaultCulture)
    {
      var logger = loggerFactory.CreateLogger(nameof(DatabaseInitializer));
      logger.LogDebug("Initializing database");

      var ticket = GetInitialTicket(dbContext, logger);

      InitializeLookups(dbContext, ticket.TicketId, logger);
      InitializeUsers(dbContext, ticket.TicketId, logger, defaultCulture);

      ticket.EndTime = DateTimeOffset.Now;
      dbContext.SaveChanges();

      logger.LogDebug("Database initialized");
    }

    private static AuditTicket GetInitialTicket(IDbContext dbContext, ILogger logger)
    {
      long? userId = null;
      if (dbContext.UserAccounts.Any())
      {
        userId = dbContext.UserAccounts
          .Where(u => u.UserName == SystemUsername.System)
          .Select(u => u.Id)
          .Single();
      }
      else
      {
        logger.LogInformation("No users found, creating userless ticket");
      }

      var ticket = new AuditTicket()
      {
        UserId = userId,
        IpAddress = "DatabaseInitializer",
        SecurityTokenIssuer = "DatabaseInitializer",
        SecurityTokenId = "DatabaseInitializer",
        StartTime = DateTimeOffset.Now
      };

      dbContext.AuditTickets.Add(ticket);
      dbContext.SaveChanges();

      return ticket;
    }

    private static void InitializeLookups(IDbContext dbContext, long ticketId, ILogger logger)
    {
      logger.LogDebug("Initializing lookups");

      logger.LogDebug("Lookups initialized");
    }

    private static void InitializeUsers(
      IDbContext dbContext, long ticketId, ILogger logger, string defaultCulture)
    {
      logger.LogDebug("Initializing users");

      AddUserIfDoesNotExist(dbContext, ticketId, SystemUsername.System, defaultCulture);
      AddUserIfDoesNotExist(dbContext, ticketId, SystemUsername.Anonymous, defaultCulture);

      logger.LogDebug("Users initialized");
    }

    private static void AddUserIfDoesNotExist(
      IDbContext dbContext, long ticketId, string username, string defaultCulture)
    {
      var userExists = dbContext.UserAccounts
        .Where(u => u.UserName == username)
        .Any();

      if (!userExists)
      {
        var principal = new Principal()
        {
          Label = username,
          PrincipalType = PrincipalType.User,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };
        dbContext.Principals.Add(principal);
        dbContext.SaveChanges();

        var principalClosureMap = new PrincipalClosureMap()
        {
          AncestorId = principal.Id,
          DescendantId = principal.Id,
          PathLength = 0,
          CreatedTicketId = ticketId
        };
        dbContext.PrincipalClosureMaps.Add(principalClosureMap);
        dbContext.SaveChanges();

        var user = new UserAccount()
        {
          Id = principal.Id,
          UserName = username,
          Culture = defaultCulture,
          CreatedTicketId = ticketId,
          ModifiedTicketId = ticketId
        };
        dbContext.UserAccounts.Add(user);
        dbContext.SaveChanges();
      }
    }
  }
}
