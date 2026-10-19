using System.Net;
using System.Net.Http.Json;
using ASPNetCoreWebAPITraining.Database;
using ASPNetCoreWebAPITraining.Models;

namespace ASPNetCoreWebAPITraining.Tests.Api;

/// <summary>
/// The behaviour every database provider must give the Person API. Each provider runs these
/// against its own throwaway database through a thin subclass.
/// </summary>
public abstract class PersonApiContractTests
{
    private const string PersonsUrl = "/api/Person";
    private const int MissingId = 9999;

    private readonly ProviderApiFactory _factory;
    private readonly HttpClient _client;

    protected PersonApiContractTests(ProviderApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPersons_ReturnsTheSeededPeople()
    {
        var people = await _client.GetFromJsonAsync<List<Person>>(PersonsUrl);

        Assert.NotNull(people);
        foreach (var seeded in SeedData.Persons)
        {
            Assert.Contains(people, p => p.Id == seeded.Id && p.FirstName == seeded.FirstName && p.LastName == seeded.LastName);
        }
    }

    [Fact]
    public async Task GetPerson_WhenItExists_ReturnsIt()
    {
        var person = await _client.GetFromJsonAsync<Person>($"{PersonsUrl}/1");

        Assert.Equal("Ada", person?.FirstName);
    }

    [Fact]
    public async Task GetPerson_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{PersonsUrl}/{MissingId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostPerson_CreatesThePersonAndPointsToIt()
    {
        var response = await _client.PostAsJsonAsync(PersonsUrl, NewPerson());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Person>();
        Assert.True(created!.Id > 0);
        Assert.EndsWith($"{PersonsUrl}/{created.Id}", response.Headers.Location!.AbsolutePath);
        await DeleteAsync(created.Id);
    }

    [Theory]
    [InlineData("""{"lastName":"Johnson"}""")]
    [InlineData("""{"firstName":"Katherine"}""")]
    [InlineData("""{"firstName":"","lastName":"Johnson"}""")]
    [InlineData("""{"firstName":"Katherine","lastName":""}""")]
    [InlineData("""{"firstName":null,"lastName":"Johnson"}""")]
    public async Task PostPerson_WithoutBothNames_ReturnsBadRequest(string json)
    {
        var response = await _client.PostAsync(PersonsUrl, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostPerson_PersistsToTheProviderDatabase()
    {
        var created = await CreateAsync();

        await using var context = _factory.CreateDbContext();
        Assert.True(await context.Persons.AnyAsync(p => p.Id == created.Id));
        await DeleteAsync(created.Id);
    }

    [Fact]
    public async Task PutPerson_UpdatesThePerson()
    {
        var created = await CreateAsync();
        created.FirstName = "Updated";

        var response = await _client.PutAsJsonAsync($"{PersonsUrl}/{created.Id}", created);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var reloaded = await _client.GetFromJsonAsync<Person>($"{PersonsUrl}/{created.Id}");
        Assert.Equal("Updated", reloaded?.FirstName);
        await DeleteAsync(created.Id);
    }

    [Fact]
    public async Task PutPerson_WhenIdsDiffer_ReturnsBadRequest()
    {
        var response = await _client.PutAsJsonAsync($"{PersonsUrl}/1", NewPerson(id: 2));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutPerson_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"{PersonsUrl}/{MissingId}", NewPerson(id: MissingId));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_RemovesThePerson()
    {
        var created = await CreateAsync();

        var response = await _client.DeleteAsync($"{PersonsUrl}/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var afterwards = await _client.GetAsync($"{PersonsUrl}/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, afterwards.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{PersonsUrl}/{MissingId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static Person NewPerson(int id = 0) =>
        new() { Id = id, FirstName = "Katherine", LastName = "Johnson" };

    private async Task<Person> CreateAsync()
    {
        var response = await _client.PostAsJsonAsync(PersonsUrl, NewPerson());
        return (await response.Content.ReadFromJsonAsync<Person>())!;
    }

    private Task DeleteAsync(int id) => _client.DeleteAsync($"{PersonsUrl}/{id}");
}

public sealed class SqlServerPersonApiTests(SqlServerApiFactory factory)
    : PersonApiContractTests(factory), IClassFixture<SqlServerApiFactory>;

public sealed class MicrosoftAccessPersonApiTests(MicrosoftAccessApiFactory factory)
    : PersonApiContractTests(factory), IClassFixture<MicrosoftAccessApiFactory>;

public sealed class SQLite3PersonApiTests(SQLite3ApiFactory factory)
    : PersonApiContractTests(factory), IClassFixture<SQLite3ApiFactory>;
