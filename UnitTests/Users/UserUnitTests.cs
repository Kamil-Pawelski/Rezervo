using Application.Abstractions.Repositories;
using Application.Users.Login;
using Application.Users.Register;
using Domain.Common;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Tests.Data;
using Tests.Seeder;

namespace Tests.Users;

public sealed class UserUnitTests
{
    private readonly PasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;
    private readonly TokenProvider _tokenProvider;
    private readonly UserRepository _userRepository;
    private readonly RoleRepository _roleRepository;
    private readonly UserRoleRepository _userRoleRepository;

    public UserUnitTests()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<UserUnitTests>()
            .Build();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _passwordHasher = new PasswordHasher();
        _tokenProvider = new TokenProvider(configuration);
        _userRepository = new UserRepository(_context);
        _roleRepository = new RoleRepository(_context);
        _userRoleRepository = new UserRoleRepository(_context);

        SeedRole.Seed(_context);
        SeedUser.Seed(_context, _passwordHasher);
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(
            "JohnDoe@test.com",
            "JohnDoe21",
            "John",
            "Doe",
            SeedUser.TestPassword,
            SeedRole.TestRoleId
        );

        Result result = await new RegisterUserCommandHandler(_passwordHasher, _userRepository, _roleRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameEmail()
    {
        var command = new RegisterUserCommand(
            SeedUser.TestUserEmail,
            "New username test",
            SeedUser.TestFirstName,
            SeedUser.TestLastName,
            SeedUser.TestPassword,
            SeedRole.TestRoleId
        );       

        Result result = await new RegisterUserCommandHandler(_passwordHasher, _userRepository, _roleRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.EmailTaken.Code);
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameUsername()
    {
        var command = new RegisterUserCommand(
            "newemail@test.com",
            SeedUser.TestUsername,
            SeedUser.TestFirstName,
            SeedUser.TestLastName,
            SeedUser.TestPassword,
            SeedRole.TestRoleId
        );

        Result result = await new RegisterUserCommandHandler(_passwordHasher, _userRepository, _roleRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.UsernameTaken.Code);
    }

    [Fact]
    public async Task LoginTest_ShouldReturnOk()
    {
        var command = new LoginUserCommand(
            SeedUser.TestUsername,
            SeedUser.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_passwordHasher, _tokenProvider, _userRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task LoginTest_ShouldReturnNotFound_WrongUsername()
    {
        var command = new LoginUserCommand(
            "EndpointTestWrong",
            SeedUser.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_passwordHasher, _tokenProvider, _userRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.NotFoundUser.Code);
    }

    [Fact]
    public async Task LoginTest_ShouldReturnUnauthorized_WrongPassword()
    {
        var command = new LoginUserCommand(
            SeedUser.TestUsername,
            "Wrong123!"
        );

        Result result = await new LoginUserCommandHandler(_passwordHasher, _tokenProvider, _userRepository, _userRoleRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.InvalidPassword.Code);
    }

}
