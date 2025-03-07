using Application.Abstractions.Authentication;
using Application.Specialists;
using Application.Specialists.Create;
using Application.Specialists.Delete;
using Application.Specialists.Get;
using Application.Specialists.GetById;
using Application.Specialists.GetBySpecialization;
using Application.Specialists.Put;
using Domain.Common;
using Domain.Specialists;
using Domain.Specializations;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Tests.Specialists;

public sealed class SpecialistsUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    private readonly TestUserContext _userContext;

    private Guid _existingSpecialistId;
    private Guid _existingUserId;
    private Guid _existingSpecializationId;

    public SpecialistsUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestConnection") 
            .Options;

        _context = new ApplicationDbContext(options);
        _userContext = new TestUserContext();

        SeedData();
    }

    private void SeedData()
    {
        _existingUserId = Guid.NewGuid();
        _existingSpecialistId = Guid.NewGuid();
        _existingSpecializationId = Guid.NewGuid();

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
            Id =  _existingSpecializationId,
            Name = "Test Specialization"
        };


        var specialist = new Specialist
        {
            Id = _existingSpecialistId,
            UserId = _existingUserId,
            SpecializationId = _existingSpecializationId,
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
        Result<List<SpecialistsResponse>> result = await new GetSpecialistsQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnSpecialist()
    {
        var query = new GetByIdSpecialistQuery(_existingSpecialistId);
        Result<SpecialistsResponse> result = await new GetByIdSpecialistQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(_existingSpecialistId);
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnNotFound()
    {
        var query = new GetByIdSpecialistQuery(Guid.NewGuid());
        Result<SpecialistsResponse> result = await new GetByIdSpecialistQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateSpecialistTest_ShouldReturnOk()
    {
        var command = new CreateSpecialistCommand(
            _existingUserId,
            _existingSpecializationId,
            "Test Description",
            "123456789",
            "Warsaw"
        );

        Result result = await new CreateSpecialistCommandHandler(_context).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
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
        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

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
        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

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
        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);
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
        Result<string> result = await new DeleteSpecialistCommandHandler(_context).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task GetBySpecialistsSpecialistTest_ShouldReturnOk()
    {
        var command = new GetBySpecializationSpecialistsCommand(_existingSpecializationId);
        Result<List<SpecialistsResponse>> result = await new GetBySpecializationSpecialistsCommandHandler(_context).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBySpecialistsSpecialistTest_ShouldReturnNotFound()
    {
        var command = new GetBySpecializationSpecialistsCommand(Guid.NewGuid());
        Result<List<SpecialistsResponse>> result = await new GetBySpecializationSpecialistsCommandHandler(_context).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
    }

    public void Dispose() => _context.Dispose();

    public class TestUserContext() : IUserContext
    {
        public Guid UserId { get; set; }
    }
}
