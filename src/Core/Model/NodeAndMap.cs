namespace DotNetBoilerplate.Core.Model
{
  using DotNetBoilerplate.Data.Entity;

  public class NodeAndMap
    : Node
  {
    public NodeAndMap()
    {
    }

    public NodeAndMap(Node nodeAndMap, int pathLength)
      : base(nodeAndMap)
    {
      this.PathLength = pathLength;
    }

    public int PathLength { get; set; }
  }
}