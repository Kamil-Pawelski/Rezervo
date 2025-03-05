using Application.Users.Login;
using Application.Users.Register;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tests.Users;

namespace Tests.Slots;

public sealed class SlotEndpointUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public SlotEndpointUnitTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<UserUnitTests>()
            .Build();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        SeedData();

    }

    private void SeedData()
    {
        
    }

    public void Dispose() => _context.Dispose();

}
