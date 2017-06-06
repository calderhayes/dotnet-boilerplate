namespace DotNetBoilerplate.Data.Model
{
  public interface IPrincipalClosureMap
  {
    long AncestorId { get; set; }

    long DescendantId { get; set; }

    int PathLength { get; set; }
  }
}