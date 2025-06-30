using Application.Abstractions.Repositories;
using Application.Specialists;
using Application.Specialists.Create;
using Application.Specialists.Delete;
using Application.Specialists.Get;
using Application.Specialists.GetById;
using Application.Specialists.GetBySpecialization;
using Application.Specialists.Put;
using Domain.Common;
using Domain.Specialists;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Tests.Data;

namespace Tests.Specialists;

public sealed class SpecialistsUnitTests
{
    private readonly ApplicationDbContext _context;
    private readonly SpecialistRepository _specialistRepository;
    private readonly SpecializationRepository _specializationRepository;

    public SpecialistsUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        _context = new ApplicationDbContext(options);
        _specialistRepository = new SpecialistRepository(_context);
        _specializationRepository = new SpecializationRepository(_context);

        SeedRole.Seed(_context);
        SeedUser.Seed(_context, new PasswordHasher());
        SeedSpecialist.Seed(_context);
    }  

    [Fact]
    public async Task GetSpecialistsTest_ShouldReturnSpecialists()
    {
        var query = new GetSpecialistsQuery();
        Result<List<SpecialistsResponse>> result = await new GetSpecialistsQueryHandler(_specialistRepository).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnSpecialist()
    {
        var query = new GetByIdSpecialistQuery(SeedSpecialist.TestSpecialistId);

        Result<SpecialistsResponse> result = await new GetByIdSpecialistQueryHandler(_specialistRepository).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetByIdSpecialistTask_ShouldReturnNotFound()
    {
        var query = new GetByIdSpecialistQuery(Guid.NewGuid());
        Result<SpecialistsResponse> result = await new GetByIdSpecialistQueryHandler(_specialistRepository).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SpecialistErrors.NotFoundSpecialist.Code);
    }

    [Fact]
    public async Task CreateSpecialistTest_ShouldReturnOk()
    {
        var command = new CreateSpecialistCommand(
            SeedUser.TestUserId,
            SeedSpecialist.TestSpecializationId,
            "Test Description",
            "123456789",
            "Warsaw"
        );

        Result result = await new CreateSpecialistCommandHandler(_specialistRepository, _specializationRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task PutSpecialistTest_ShouldReturnForbidden()
    {
        var command = new PutSpecialistCommand(
            SeedUser.TestUserId,
            SeedSpecialist.TestSpecializationId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_specialistRepository, new TestUserContext { UserId = Guid.NewGuid() }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }


    [Fact]
    public async Task PutSpecialistTest_ShouldReturnOk()
    {
        var command = new PutSpecialistCommand(
            SeedSpecialist.TestSpecialistId,
            SeedUser.TestUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_specialistRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.City.ShouldBe("Cracow");
    }

    [Fact]
    public async Task PutSpecialistTest_ShouldReturnNotFound()
    {
        var command = new PutSpecialistCommand(
            Guid.NewGuid(),
            SeedUser.TestUserId,
            "Test Description",
            "123456789",
            "Cracow"
        );

        Result<SpecialistsResponse> result = await new PutSpecialistCommandHandler(_specialistRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SpecialistErrors.NotFoundSpecialist.Code);
    }

    [Fact]
    public async Task DeleteSpecialistTest_ShouldReturnNotFound()
    {
        var command = new DeleteSpecialistCommand(Guid.NewGuid());
        Result<string> result = await new DeleteSpecialistCommandHandler(_specialistRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SpecialistErrors.NotFoundSpecialist.Code);
    }

    [Fact]
    public async Task DeleteSpecialistTest_ShouldReturnOk()
    {
        var command = new DeleteSpecialistCommand(SeedSpecialist.TestSpecialistToDeleteId);
        Result<string> result = await new DeleteSpecialistCommandHandler(_specialistRepository).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task GetBySpecialistsSpecialistTest_ShouldReturnOk()
    {
        var command = new GetBySpecializationSpecialistsCommand(SeedSpecialist.TestSpecializationId);
        Result<List<SpecialistsResponse>> result = await new GetBySpecializationSpecialistsCommandHandler(_specialistRepository).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBySpecialistsSpecialistTest_ShouldReturnNotFound()
    {
        var command = new GetBySpecializationSpecialistsCommand(Guid.NewGuid());
        Result<List<SpecialistsResponse>> result = await new GetBySpecializationSpecialistsCommandHandler(_specialistRepository).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SpecialistErrors.NotFoundSpecialist.Code);
    }
}
