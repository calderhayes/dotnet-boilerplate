namespace DotNetBoilerplate.Api.Utility.Tests
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Primitives;
  using Xunit;

  public class RequestFunctionsTests
  {
    [Fact]
    public void GetHeaderValueAs_FindNonExistantHeaderValue()
    {
      var httpContext = new DefaultHttpContext();
      var val = RequestFunctions.GetHeaderValueAs<int>(httpContext, "DoesNotExist");
      Assert.True(default(int) == val, "Must return the type default for non-existant headers");
    }

    [Fact]
    public void GetHeaderValueAs_FindExistandHeaderString()
    {
      var httpContext = new DefaultHttpContext();
      var headerName = "DummyHeader";
      var headerValue = "HeaderValue";
      httpContext.Request.Headers.Add(headerName, headerValue);
      var val = RequestFunctions.GetHeaderValueAs<string>(httpContext, headerName);
      Assert.True(headerValue == val, "Must retrieve the correct header value for a header name");
    }

    [Fact]
    public void GetHeaderValueAs_FindExistandHeaderStrings()
    {
      var httpContext = new DefaultHttpContext();
      var headerName = "DummyHeader";
      var headerValue = new StringValues(new string[] { "HeaderValue1", "HeaderValue2" });
      httpContext.Request.Headers.Add(headerName, headerValue);
      var val = RequestFunctions.GetHeaderValueAs<string>(httpContext, headerName);
      Assert.True(headerValue.ToString() == val, "Must retrieve the correct header values");
    }
  }
}
