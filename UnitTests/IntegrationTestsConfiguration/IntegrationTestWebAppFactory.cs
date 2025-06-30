using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Web.Api;

namespace Tests.IntegrationTestsConfiguration;

public class IntegrationTestWebAppFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("P@ssw0rd123")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder) => 
        builder.ConfigureTestServices(services =>
        {
            Type descriptorType = typeof(DbContextOptions<ApplicationDbContext>);

            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == descriptorType);

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var connectionString = _container.GetConnectionString() + ";Database=MyTestDatabase;";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        });

    public Task InitializeAsync() => _container.StartAsync();

    public new Task DisposeAsync() => _container.StopAsync();
}

