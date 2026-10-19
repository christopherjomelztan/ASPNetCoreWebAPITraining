using System.Net;
using System.Text.Json;

namespace ASPNetCoreWebAPITraining.Tests.Api;

public sealed class ApiDocumentationTests : IClassFixture<SQLite3ApiFactory>
{
    private const string OpenApiUrl = "/openapi/v1.json";

    private readonly HttpClient _client;

    public ApiDocumentationTests(SQLite3ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task OpenApiDocument_DescribesThePersonApi()
    {
        using var document = JsonDocument.Parse(await _client.GetStringAsync(OpenApiUrl));

        Assert.Equal("Person API", document.RootElement.GetProperty("info").GetProperty("title").GetString());
        Assert.True(document.RootElement.GetProperty("paths").TryGetProperty("/api/Person", out _));
    }

    [Fact]
    public async Task Redoc_IsServedAndPointsAtTheOpenApiDocument()
    {
        var response = await _client.GetAsync("/redoc");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains($"spec-url=\"{OpenApiUrl}\"", html);
    }

    [Fact]
    public async Task SwaggerUi_IsNoLongerServed()
    {
        var response = await _client.GetAsync("/swagger/index.html");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
