using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Users;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Users;
using Web.Api;
using DependencyInjection = Infrastructure.DependencyInjection;

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

            services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            using IServiceScope scope = services.BuildServiceProvider().CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            SeedTestData(db, passwordHasher);
        });

        builder.UseEnvironment("Test");
    }
    private static void SeedTestData(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
    {
        User user = new()
        {
            Email = "EndpointTest@example.com",
            Username = "EndpointTest",
            FirstName = "Endpoint",
            LastName = "Test",
            PasswordHash = passwordHasher.Hash("Password123!")
        };

        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }
}

[CollectionDefinition("Factory")]
public class SharedCustomWebApplicationFactory : IClassFixture<CustomWebApplicationFactory<Program>>
{
}
