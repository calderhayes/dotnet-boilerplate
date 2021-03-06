namespace DotNetBoilerplate.Data.Entity
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using DotNetBoilerplate.Data.Model;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Defines a base node object
  /// </summary>
  public class Node
    : BaseEntity, INode
  {
    public Node()
    {
    }

    public Node(Node node)
      : base(node)
    {
      this.Label = node.Label;
      this.NodeType = node.NodeType;
    }

    [Required]
    public string Label { get; set; }

    [Required]
    public NodeType NodeType { get; set; }
  }
}