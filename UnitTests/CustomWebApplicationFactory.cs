using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Specialists;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Specialists;
using Tests.Users;
using Web.Api;

namespace Tests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json");
            config.AddUserSecrets<UserEndpointTests>();
            config.AddUserSecrets<SpecialistsEndpointTests>();
        });

        builder.ConfigureServices((context, services) =>
        {
            string? connectionString = context.Configuration.GetConnectionString("TestConnection");
            ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Change it to add infrastructure

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IApplicationDbContext>(serviceProvider =>
                serviceProvider.GetRequiredService<ApplicationDbContext>());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddHttpContextAccessor();

            using IServiceScope scope = services.BuildServiceProvider().CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            SeedData.SeedUserTestData(db, passwordHasher);
            SeedData.SeedSpecialistTestData(db);
        });

        builder.UseEnvironment("Test");
    }
}

[CollectionDefinition("Factory")]
public class SharedCustomWebApplicationFactory : IClassFixture<CustomWebApplicationFactory<Program>>
{
}
