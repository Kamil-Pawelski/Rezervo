using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Seeder;

namespace Tests.IntegrationTestsConfiguration;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime, IDisposable
{
    private readonly IServiceScope _scope; 
    protected readonly ApplicationDbContext DbContext;
    protected readonly HttpClient Client;
    private readonly IntegrationTestWebAppFactory _factory;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.MigrateAsync();
        await SeedData.Initialize(DbContext);
    }

    public Task DisposeAsync() =>
        Task.CompletedTask;

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
