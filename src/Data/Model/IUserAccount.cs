namespace DotNetBoilerplate.Data.Model
{
  public interface IUserAccount
  {
    long Id { get; set; }

    string UserName { get; set; }

    string Culture { get; set; }
  }
}
