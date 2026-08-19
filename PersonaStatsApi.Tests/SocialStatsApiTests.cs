using System.Net;
using System.Net.Http.Json;
using PersonaStatsApi.Models;
using Xunit;

namespace PersonaStatsApi.Tests;

public class SocialStatsApiTests : IClassFixture<SocialStatsApiFactory>
{
    private readonly HttpClient _client;

    public SocialStatsApiTests(SocialStatsApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsSeededStats()
    {
        var response = await _client.GetAsync("/api/socialstats");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var stats = await response.Content.ReadFromJsonAsync<List<SocialStats>>();
        Assert.NotNull(stats);
        Assert.Equal(5, stats!.Count);
    }

    [Fact]
    public async Task GetById_Existing_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/socialstats/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var stat = await response.Content.ReadFromJsonAsync<SocialStats>();
        Assert.NotNull(stat);
        Assert.Equal("Conocimiento", stat!.Name);
    }

    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        var response = await _client.GetAsync("/api/socialstats/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var newStat = new SocialStats { Name = "Encanto", Level = 2, Points = 50 };

        var response = await _client.PostAsJsonAsync("/api/socialstats", newStat);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<SocialStats>();
        Assert.NotNull(created);
        Assert.Equal("Encanto", created!.Name);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_InvalidData_Returns400()
    {
        var newStat = new SocialStats { Name = "Fuerza", Level = 99, Points = 50 };

        var response = await _client.PostAsJsonAsync("/api/socialstats", newStat);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_Existing_ReturnsNoContent()
    {
        var updated = new SocialStats { Id = 1, Name = "Conocimiento", Level = 3, Points = 100 };

        var response = await _client.PutAsJsonAsync("/api/socialstats/1", updated);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_IdMismatch_Returns400()
    {
        var updated = new SocialStats { Id = 2, Name = "Coraje", Level = 2, Points = 10 };

        var response = await _client.PutAsJsonAsync("/api/socialstats/1", updated);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_NotFound_Returns404()
    {
        var updated = new SocialStats { Id = 999, Name = "Inexistente", Level = 1, Points = 0 };

        var response = await _client.PutAsJsonAsync("/api/socialstats/999", updated);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Existing_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/api/socialstats/5");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotFound_Returns404()
    {
        var response = await _client.DeleteAsync("/api/socialstats/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
    }
}