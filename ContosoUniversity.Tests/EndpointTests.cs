using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Tests;

public sealed class EndpointTests : IClassFixture<ContosoUniversityFactory>
{
    private readonly HttpClient _client;

    public EndpointTests(ContosoUniversityFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/", "ASP.NET Core MVC and Entity Framework Core on .NET 10")]
    [InlineData("/Home/About", "Student Body Statistics")]
    [InlineData("/Students", "Alexander")]
    [InlineData("/Courses", "Chemistry")]
    [InlineData("/Instructors", "Abercrombie")]
    [InlineData("/Departments", "English")]
    public async Task Required_endpoint_returns_seeded_data(string path, string expectedContent)
    {
        var response = await _client.GetAsync(path);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedContent, content);
    }
}

public sealed class ContosoUniversityFactory : WebApplicationFactory<Program>
{
    private readonly string _databasePath = Path.Combine(
        Path.GetTempPath(),
        $"contoso-university-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={_databasePath}"
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            File.Delete(_databasePath);
        }
    }
}
