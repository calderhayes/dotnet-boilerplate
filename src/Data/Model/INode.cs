namespace DotNetBoilerplate.Data.Model
{
  using DotNetBoilerplate.Data.Entity;
  using DotNetBoilerplate.Data.Model.Lookup;

  public interface INode
    : IBaseEntity
  {
    string Label { get; set; }

    NodeType NodeType { get; set; }
  }
}
