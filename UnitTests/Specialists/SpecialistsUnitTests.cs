using Application.Specialists.Create;
using Application.Specialists.Get;
using Domain.Common;
using Domain.Specialists;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Tests.Specialists;

public sealed class SpecialistsUnitTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetSpecialistsQueryHandler _getSpecialistsQueryHandler;
    private readonly CreateSpecialistCommandHandler _createSpecialistCommandHandler;
    public SpecialistsUnitTests()
    {

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _createSpecialistCommandHandler = new CreateSpecialistCommandHandler(_context);
        _getSpecialistsQueryHandler = new GetSpecialistsQueryHandler(_context);
        SeedData();
    }

    private void SeedData()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Email = "EndpointTest@example.com",
            Username = "EndpointTest",
            FirstName = "Endpoint",
            LastName = "Test",
            PasswordHash = "Password123!"
        };

        var specialization = new Specialization
        {
            Id = Guid.NewGuid(),
            Name = "Test Specialization"
        };

        var specialist = new Specialist
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            SpecializationId = specialization.Id,
            Description = "Test Description",
            PhoneNumber = "123456789"
        };

        _context.Specializations.Add(specialization);
        _context.Specialists.Add(specialist);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetSpecialistsTest_ShouldReturnSpecialists()
    {
        var query = new GetSpecialistsQuery();
        Result<List<SpecialistsResponse>> result = await _getSpecialistsQueryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task CreateSpecialistTest_ShouldReturnOk()
    {
        var command = new CreateSpecialistCommand(
            Guid.NewGuid(),
            "Test Specialization",
            "Test Description",
            "123456789"
        );

        Result result = await _createSpecialistCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}
