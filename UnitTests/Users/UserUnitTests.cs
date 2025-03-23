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

namespace Tests.Users;

public sealed class UserUnitTests
{
    private readonly PasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;
    private readonly TokenProvider _tokenProvider;
    private readonly UserRepository _userRepository;

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

        SeedData.SeedRoleData(_context);
        SeedData.SeedUserTestData(_context, _passwordHasher);
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(
            "JohnDoe@test.com",
            "JohnDoe21",
            "John",
            "Doe",
            SeedData.TestPassword,
            SeedData.TestRoleId
        );

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameEmail()
    {
        var command = new RegisterUserCommand(
            SeedData.TestUserEmail,
            "New username test",
            SeedData.TestFirstName,           
            SeedData.TestLastName,
            SeedData.TestPassword,
            SeedData.TestRoleId
        );       

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.EmailTaken.Code);
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameUsername()
    {
        var command = new RegisterUserCommand(
            "newemail@test.com",
            SeedData.TestUsername,
            SeedData.TestFirstName,
            SeedData.TestLastName,
            SeedData.TestPassword,
            SeedData.TestRoleId
        );

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.UsernameTaken.Code);
    }

    [Fact]
    public async Task LoginTest_ShouldReturnOk()
    {
        var command = new LoginUserCommand(
            SeedData.TestUsername,
            SeedData.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task LoginTest_ShouldReturnNotFound_WrongUsername()
    {
        var command = new LoginUserCommand(
            "EndpointTestWrong",
            SeedData.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.NotFoundUser.Code);
    }

    [Fact]
    public async Task LoginTest_ShouldReturnUnauthorized_WrongPassword()
    {
        var command = new LoginUserCommand(
            SeedData.TestUsername,
            "Wrong123!"
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider, _userRepository).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(UserErrors.InvalidPassword.Code);
    }

}
