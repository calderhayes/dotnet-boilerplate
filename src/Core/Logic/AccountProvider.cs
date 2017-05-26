namespace DotNetBoilerplate.Core.Logic
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  /// <summary>
  ///
  /// </summary>
  public class AccountProvider
    : IAccountProvider
  {
    /// <summary>
    ///
    /// </summary>
    private static Random random = new Random();

    /// <summary>
    ///
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public AccountProvider(
      IDbContext dbContext,
      ILogger<AccountProvider> logger)
    {
      this.Logger = logger;
      this.DbContext = dbContext;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private IDbContext DbContext { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private ILogger Logger { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="authSource"></param>
    /// <param name="sub"></param>
    /// <returns></returns>
    public async Task<IUserAccount> GetUserByAuthSourceAndSub(
      string authSource, string sub)
    {
      var query = from uas in this.DbContext.UserAuthenticationSources
      join ua in this.DbContext.UserAccounts on uas.UserId equals ua.UserId
      where uas.AuthenticationSource == authSource && uas.Subject == sub
      select ua;

      return await query.SingleOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="auditTicketId"></param>
    /// <param name="authSource"></param>
    /// <param name="sub"></param>
    /// <param name="preferredUsername"></param>
    /// <param name="email"></param>
    /// <param name="locale"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public async Task<IUserAccount> RegisterUser(
      long auditTicketId,
      string authSource,
      string sub,
      string preferredUsername,
      string email,
      string locale,
      IList<string> roles)
    {
      var userAccount = new UserAccount()
      {
        UserName = await this.GetUniqueUsername(preferredUsername.Trim()),
        Culture = locale,
        CreatedTicketId = auditTicketId,
        ModifiedTicketId = auditTicketId
      };

      this.DbContext.UserAccounts.Add(userAccount);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);

      var authenticationSource = new UserAuthenticationSource()
      {
        AuthenticationSource = authSource,
        Subject = sub,
        UserId = userAccount.UserId,
        CreatedTicketId = auditTicketId,
        ModifiedTicketId = auditTicketId
      };
      this.DbContext.UserAuthenticationSources.Add(authenticationSource);
      await this.DbContext.SaveChangesAsync(CancellationToken.None);

      return userAccount;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<IUserAccount> GetAnonymousAccount()
    {
      var anonymous = await this.DbContext.UserAccounts
          .Where(u => u.UserName == Data.Constants.SystemUsername.Anonymous)
          .SingleAsync();
      return anonymous;
    }

    /// <summary>
    /// NOT cryptographically sound
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="preferredUsername"></param>
    /// <returns></returns>
    private async Task<string> GetUniqueUsername(string preferredUsername)
    {
      var username = preferredUsername;

      bool doesUsernameExist = true;
      while (!doesUsernameExist)
      {
        doesUsernameExist = await this.DbContext.UserAccounts
          .Where(ua => ua.UserName == username)
          .AnyAsync();

        if (doesUsernameExist)
        {
          username = preferredUsername + RandomString(4);
        }
      }

      return username;
    }
  }
}
