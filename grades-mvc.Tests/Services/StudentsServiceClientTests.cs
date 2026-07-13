using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;

namespace grades_mvc.Tests.Services;

public class StudentsServiceClientTests : IDisposable
{
    private readonly string _dbName;
    private readonly Mock<ILogger<StudentsServiceClient>> _loggerMock;
    private readonly ServiceProvider _serviceProvider;

    public StudentsServiceClientTests()
    {
        _dbName = Guid.NewGuid().ToString();
        _loggerMock = new Mock<ILogger<StudentsServiceClient>>();

        var services = new ServiceCollection();
        services.AddDbContext<GradesDbContext>(opt => opt.UseInMemoryDatabase(_dbName));
        services.AddDbContextFactory<GradesDbContext>(opt => opt.UseInMemoryDatabase(_dbName));
        _serviceProvider = services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GradesDbContext>();
        GradesTestDbHelper.SeedTestData(context);
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    private StudentsServiceClient CreateClientWithHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    {
        var handler = new MockHttpMessageHandler(handlerFunc);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };
        return new StudentsServiceClient(httpClient, _loggerMock.Object, _serviceProvider);
    }

    [Fact]
    public async Task GetStudentByIdAsync_Success_ReturnsStudent()
    {
        var client = CreateClientWithHandler(request =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"id":1,"firstName":"Ahmed","lastName":"Ali","email":"ahmed@test.com"}""")
            };
            return Task.FromResult(response);
        });

        var result = await client.GetStudentByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Ahmed", result.FirstName);
    }

    [Fact]
    public async Task GetStudentByIdAsync_NotFound_ReturnsNull()
    {
        var client = CreateClientWithHandler(request =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            return Task.FromResult(response);
        });

        var result = await client.GetStudentByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ServerError_FallsBackToCache()
    {
        var client = CreateClientWithHandler(request =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return Task.FromResult(response);
        });

        var result = await client.GetStudentByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Ahmed", result.FirstName);
    }

    [Fact]
    public async Task GetAllStudentsAsync_Success_ReturnsStudents()
    {
        var client = CreateClientWithHandler(request =>
        {
            var json = """[{"id":1,"firstName":"Ahmed","lastName":"Ali","email":"ahmed@test.com"},{"id":2,"firstName":"Mohamed","lastName":"Hassan","email":"mohamed@test.com"}]""";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            return Task.FromResult(response);
        });

        var result = await client.GetAllStudentsAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllStudentsAsync_ServerError_FallsBackToCache()
    {
        var client = CreateClientWithHandler(request =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return Task.FromResult(response);
        });

        var result = await client.GetAllStudentsAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetStudentsByIdsAsync_ReturnsMatchingStudents()
    {
        var client = CreateClientWithHandler(request =>
        {
            var json = """{"id":1,"firstName":"Ahmed","lastName":"Ali","email":"ahmed@test.com"}""";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            return Task.FromResult(response);
        });

        var result = await client.GetStudentsByIdsAsync(new[] { 1, 1, 2 });

        Assert.True(result.ContainsKey(1));
    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerFunc;

    public MockHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    {
        _handlerFunc = handlerFunc;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _handlerFunc(request);
    }
}
