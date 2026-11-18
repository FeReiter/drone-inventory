using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DroneInventory.Api.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DroneInventory.Api.Tests;

public sealed class ApiProblemDetailsTests : IAsyncLifetime
{
    private WebApplication app = null!;
    private HttpClient client = null!;

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Services.AddApiProblemDetails();
        app = builder.Build();
        app.UseApiProblemDetails();

        app.MapGet("/field-error", () => TypedResults.ValidationProblem(
            new Dictionary<string, string[]> { ["name"] = ["Der Name ist erforderlich."] },
            detail: "Bitte korrigiere die markierten Felder."));
        app.MapGet("/conflict", () => TypedResults.Problem(
            statusCode: StatusCodes.Status409Conflict,
            title: "Loan-Konflikt",
            detail: "Für diese Drone besteht bereits eine aktive Loan."));
        app.MapGet("/technical-error", (HttpContext _) => throw new InvalidOperationException("Interne Testdetails"));
        await app.StartAsync();
        client = new HttpClient { BaseAddress = new Uri(app.Urls.Single()) };
    }

    [Theory]
    [InlineData("/field-error", HttpStatusCode.BadRequest)]
    [InlineData("/unknown", HttpStatusCode.NotFound)]
    [InlineData("/conflict", HttpStatusCode.Conflict)]
    [InlineData("/technical-error", HttpStatusCode.InternalServerError)]
    public async Task ReturnsProblemJsonForHttpErrors(string path, HttpStatusCode status)
    {
        using var response = await client.GetAsync(path);
        Assert.Equal(status, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal((int)status, problem.GetProperty("status").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("title").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("detail").GetString()));

        if (status == HttpStatusCode.BadRequest)
        {
            var errors = problem.GetProperty("errors").GetProperty("name");
            Assert.Equal("Der Name ist erforderlich.", Assert.Single(errors.EnumerateArray()).GetString());
        }
        else
        {
            Assert.False(problem.TryGetProperty("errors", out _));
        }

        Assert.DoesNotContain("Interne Testdetails", problem.GetRawText());
    }

    public async Task DisposeAsync()
    {
        client?.Dispose();
        if (app is not null)
        {
            await app.DisposeAsync();
        }
    }
}
