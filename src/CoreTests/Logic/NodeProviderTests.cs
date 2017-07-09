namespace DotNetBoilerplate.Core.Logic.Tests
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using DotNetBoilerplate.Data;
  using DotNetBoilerplate.Data.Model.Lookup;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;
  using Moq;
  using Xunit;

  public class NodeProviderTests
  {
    [Fact]
    public async Task CreateNode_BasicTest()
    {
      var options = new DbContextOptionsBuilder<PostgreSQLContext>()
        .UseInMemoryDatabase(databaseName: nameof(CreateNode_BasicTest))
        .Options;

      // Test data
      long ticketId = 23213;
      var label = "testnode";
      var nodeType = NodeType.User;

      // Mocked objects
      var logger = new Mock<ILogger<NodeProvider>>();

      using (var context = new PostgreSQLContext(options))
      {
        // Run tests
        var nodeProvider = new NodeProvider(context, logger.Object);
        var node = await nodeProvider.CreateNode(label, nodeType, ticketId);
        Assert.NotNull(node);
        Assert.NotEqual(default(long), node.Id);
        Assert.StrictEqual(label, node.Label);
        Assert.StrictEqual(nodeType, node.NodeType);
        Assert.StrictEqual(ticketId, node.CreatedTicketId);
        Assert.StrictEqual(ticketId, node.ModifiedTicketId);
        Assert.StrictEqual(false, node.IsDeleted);
        Assert.NotEqual(default(Guid), node.ExternalId);
      }
    }

    [Fact]
    public async Task AddChildNode_BasicTest()
    {
      var options = new DbContextOptionsBuilder<PostgreSQLContext>()
        .UseInMemoryDatabase(databaseName: nameof(AddChildNode_BasicTest))
        .Options;

      // Test data
      long ticketId = 54264;
      var label1 = "testnode";
      var nodeType = NodeType.User;

      var label2 = "testnode222";

      // Mocked objects
      var logger = new Mock<ILogger<NodeProvider>>();

      using (var context = new PostgreSQLContext(options))
      {
        // Run tests
        var nodeProvider = new NodeProvider(context, logger.Object);
        var node1 = await nodeProvider.CreateNode(label1, nodeType, ticketId);
        var node2 = await nodeProvider.CreateNode(label2, nodeType, ticketId);

        Assert.NotEqual(node1.Id, node2.Id);
        Assert.NotEqual(node1.Label, node2.Label);

        await nodeProvider.AddChildNode(
          ticketId, node1.Id, node2.Id, NodeClosureMapDomain.SecurityProfileAssignment);
      }
    }

    [Fact]
    public async Task GetAncestorsQuery_BasicTest()
    {
      var options = new DbContextOptionsBuilder<PostgreSQLContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetAncestorsQuery_BasicTest))
        .Options;

      // Test data
      long ticketId = 542532;
      var label1 = "testnode";
      var nodeType = NodeType.User;

      var label2 = "testnode222";

      // Mocked objects
      var logger = new Mock<ILogger<NodeProvider>>();

      using (var context = new PostgreSQLContext(options))
      {
        // Run tests
        var nodeProvider = new NodeProvider(context, logger.Object);
        var node1 = await nodeProvider.CreateNode(label1, nodeType, ticketId);
        var node2 = await nodeProvider.CreateNode(label2, nodeType, ticketId);

        Assert.NotEqual(node1.Id, node2.Id);
        Assert.NotEqual(node1.Label, node2.Label);

        await nodeProvider.AddChildNode(
          ticketId, node1.Id, node2.Id, NodeClosureMapDomain.SecurityProfileAssignment);

        var ancestors = await nodeProvider.GetAncestorsQuery(
          node1.Id, NodeClosureMapDomain.SecurityProfileAssignment, true)
          .ToListAsync();

        Assert.StrictEqual(1, ancestors.Count);

        /*var ancestor = ancestors.First();
        Assert.StrictEqual(0, ancestor.PathLength);
        Assert.StrictEqual(node1.Id, ancestor.Id);

        ancestors = await nodeProvider.GetAncestorsQuery(
          node1.Id, NodeClosureMapDomain.SecurityProfileAssignment, false)
          .ToListAsync();

        Assert.StrictEqual(0, ancestors.Count);

        ancestors = await nodeProvider.GetAncestorsQuery(
          node2.Id, NodeClosureMapDomain.SecurityProfileAssignment, true)
          .ToListAsync();

        Assert.StrictEqual(2, ancestors.Count);*/
      }
    }

  }
}