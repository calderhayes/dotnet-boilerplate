namespace DotNetBoilerplate.Api.Utility
{
  using System;
  using System.Reflection;
  using Microsoft.AspNetCore.SignalR.Infrastructure;
  using Newtonsoft.Json.Serialization;

  /// <summary>
  /// Taken from:
  /// https://radu-matei.github.io/blog/aspnet-core-mvc-signalr/
  /// </summary>
  public class SignalRContractResolver
    : IContractResolver
  {
    private readonly Assembly assembly;
    private readonly IContractResolver camelCaseContractResolver;
    private readonly IContractResolver defaultContractSerializer;

    public SignalRContractResolver()
    {
      this.defaultContractSerializer = new DefaultContractResolver();
      this.camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
      this.assembly = typeof(Connection).GetTypeInfo().Assembly;
    }

    public JsonContract ResolveContract(Type type)
    {
      if (type.GetTypeInfo().Assembly.Equals(this.assembly))
      {
        return this.defaultContractSerializer.ResolveContract(type);
      }

      return this.camelCaseContractResolver.ResolveContract(type);
    }
  }
}