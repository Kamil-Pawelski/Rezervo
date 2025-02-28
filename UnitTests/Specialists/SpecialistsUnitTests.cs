using Application.Abstractions.Authentication;
using Application.Specialists;
using Application.Specialists.Create;
using Application.Specialists.Delete;
using Application.Specialists.Get;
using Application.Specialists.GetById;
using Application.Specialists.Put;
using Domain.Common;
using Domain.Specialists;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Tests.Specialists;

public sealed class SpecialistsUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly GetSpecialistsQueryHandler _getSpecialistsQueryHandler;
    private readonly GetByIdSpecialistQueryHandler _getByIdSpecialistsQueryHandler;
    private readonly CreateSpecialistCommandHandler _createSpecialistCommandHandler;
    private readonly PutSpecialistCommandHandler _putSpecialistCommandHandler;
    private readonly TestUserContext _userContext;

    private Guid _existingSpecialistId;
    private Guid _existingUserId;

    public SpecialistsUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestConnection") 
            .Options;

        _context = new ApplicationDbContext(options);
        _userContext = new TestUserContext();

        _createSpecialistCommandHandler = new CreateSpecialistCommandHandler(_context);
        _getSpecialistsQueryHandler = new GetSpecialistsQueryHandler(_context);
        _getByIdSpecialistsQueryHandler = new GetByIdSpecialistQueryHandler(_context);
        _putSpecialistCommandHandler = new PutSpecialistCommandHandler(_context, _userContext);

        SeedData();
    }

    private void SeedData()
    {
        _existingUserId = Guid.NewGuid();
        var user = new User
        {
            Id = _existingUserId,
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

        _existingSpecialistId = Guid.NewGuid();
        var specialist = new Specialist
        {
            Id = _existingSpecialistId,
            UserId = _existingUserId,
            SpecializationId = specialization.Id,
            Description = "Test Description",
            PhoneNumber = "123456789",
            City = "Warsaw"
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
        result.Value.ShouldNotBeNull();
        result.Value.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnSpecialist()
    {
        var query = new GetByIdSpecialistQuery(_existingSpecialistId);
        Result<SpecialistsResponse> result = await _getByIdSpecialistsQueryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(_existingSpecialistId);
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnNotFound()
    {
        var query = new GetByIdSpecialistQuery(Guid.NewGuid());
        Result<SpecialistsResponse> result = await _getByIdSpecialistsQueryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateSpecialistTest_ShouldReturnOk()
    {
        var command = new CreateSpecialistCommand(
            _existingUserId, 
            "Test Specialization",
            "Test Description",
            "123456789",
            "Warsaw"
        );

        Result result = await _createSpecialistCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task PutSpecialistTest_ShouldReturnUnauthorized()
    {
        var command = new PutSpecialistCommand(
            _existingSpecialistId,
            _existingUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        _userContext.UserId = Guid.Empty;
        Result<SpecialistsResponse> result = await _putSpecialistCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task PutSpecialistTest_ShouldReturnForbidden()
    {
        var command = new PutSpecialistCommand(
            _existingSpecialistId,
            _existingUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        _userContext.UserId = Guid.NewGuid();
        Result<SpecialistsResponse> result = await _putSpecialistCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.Forbidden);
    }


    [Fact]
    public async Task PutSpecialistTest_ShouldReturnOk()
    {
        var command = new PutSpecialistCommand(
            _existingSpecialistId,
            _existingUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        _userContext.UserId = _existingUserId;
        Result<SpecialistsResponse> result = await _putSpecialistCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.City.ShouldBe("Cracow");
    }

    [Fact]
    public async Task PutSpecialistTest_ShouldReturnNotFound()
    {
        var command = new PutSpecialistCommand(
            Guid.NewGuid(),
            _existingUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );
        _userContext.UserId = _existingUserId;
        Result<SpecialistsResponse> result = await _putSpecialistCommandHandler.Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteSpecialistTest_ShouldReturnNotFound()
    {
        var command = new DeleteSpecialistCommand(Guid.NewGuid());
        Result<string> result = await new DeleteSpecialistCommandHandler(_context).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteSpecialistTest_ShouldReturnOk()
    {
        var command = new DeleteSpecialistCommand(_existingSpecialistId);
        Result<string> result =
            await new DeleteSpecialistCommandHandler(_context).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    public class TestUserContext() : IUserContext
    {
        public Guid UserId { get; set; }
    }
}
