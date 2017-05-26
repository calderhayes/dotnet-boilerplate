
namespace DotNetBoilerplate.Core.Logic.Tests
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using Microsoft.EntityFrameworkCore;
  using Xunit;
  using Moq;
  using Microsoft.Extensions.Logging;

  public class AccountProviderTests
  {
    [Fact]
    public async Task GetUserByAuthSourceAndSub_DoesFindsAMatch()
    {
      var options = new DbContextOptionsBuilder<PostgreSQLContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetUserByAuthSourceAndSub_DoesFindsAMatch))
        .Options;

      // Test Data
      var authSource = "google";
      var subject = "141132fafas";
      long userId = 1213100;
      long createdTicketId = 4424;
      long modifiedTicketId = 4230;

      var userAccount = new UserAccount()
      {
        UserId = userId,
        UserName = "testuser",
        Culture = "en-CA",
        CreatedTicketId = createdTicketId,
        ModifiedTicketId = modifiedTicketId
      };

      // Mock setup
      using (var context = new PostgreSQLContext(options))
      {
        var userSource = new UserAuthenticationSource()
        {
          AuthenticationSource = authSource,
          Subject = subject,
          UserId = userId,
          CreatedTicketId = createdTicketId,
          ModifiedTicketId = modifiedTicketId
        };
        context.UserAuthenticationSources.Add(userSource);
        await context.SaveChangesAsync();

        context.UserAccounts.Add(userAccount);
        await context.SaveChangesAsync();
      }

      var logger = new Mock<ILogger<AccountProvider>>();

      // Execute method
      using (var context = new PostgreSQLContext(options))
      {
        var accountProvider = new AccountProvider(context, logger.Object);
        var match = await accountProvider.GetUserByAuthSourceAndSub(authSource, subject);
        Assert.True(match != null, "Must find a match");
        Assert.True(match.UserId == userId, "User IDs must match");
        Assert.True(match.UserName == userAccount.UserName);
        Assert.True(match.Culture == userAccount.Culture);
      }
    }

    [Fact]
    public async Task GetUserByAuthSourceAndSub_DoesNotFindAMatch()
    {
      var options = new DbContextOptionsBuilder<PostgreSQLContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetUserByAuthSourceAndSub_DoesNotFindAMatch))
        .Options;

      // Test Data
      var authSource = "google";
      var subject = "141132fafas";

      var logger = new Mock<ILogger<AccountProvider>>();

      // Execute method
      using (var context = new PostgreSQLContext(options))
      {
        var accountProvider = new AccountProvider(context, logger.Object);
        var match = await accountProvider.GetUserByAuthSourceAndSub(authSource, subject);
        Assert.True(match == null, "Must not find a match");
      }
    }
  }
}
