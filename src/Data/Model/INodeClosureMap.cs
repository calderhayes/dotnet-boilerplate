namespace DotNetBoilerplate.Data.Model
{
  public interface INodeClosureMap
  {
    long AncestorId { get; set; }

    long DescendantId { get; set; }

    int PathLength { get; set; }
  }
}