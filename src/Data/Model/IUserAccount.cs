namespace DotNetBoilerplate.Data.Model
{
  public interface IUserAccount
  {
    long UserId { get; set; }

    string UserName { get; set; }

    string Culture { get; set; }
  }
}
