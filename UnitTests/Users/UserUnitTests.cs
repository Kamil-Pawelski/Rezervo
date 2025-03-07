using Application.Users.Login;
using Application.Users.Register;
using Domain.Common;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace Tests.Users;

public sealed class UserUnitTests : IDisposable
{
    private readonly RegisterUserCommandHandler _registerUserCommandHandler;
    private readonly LoginUserCommandHandler _loginUserCommandHandler;
    private readonly PasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;

    private Guid _existingRoleId;

    public UserUnitTests()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<UserUnitTests>()
            .Build();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _passwordHasher = new PasswordHasher();
        var tokenProvider = new TokenProvider(configuration, _context);

        _registerUserCommandHandler = new RegisterUserCommandHandler(_context, _passwordHasher);
        _loginUserCommandHandler = new LoginUserCommandHandler(_context, _passwordHasher, tokenProvider);
        SeedData();
    }

    private void SeedData()
    {
        User user = new()
        {
            Email = "EndpointTest@example.com",
            Username = "EndpointTest",
            FirstName = "Endpoint",
            LastName = "Test",
            PasswordHash = _passwordHasher.Hash("Password123!")
        };

        _existingRoleId = Guid.NewGuid();

        var clientRole = new Role
        {
            Id = _existingRoleId,
            Name = RolesNames.Client
        };

        _context.Users.Add(user);
        _context.Roles.Add(clientRole);
        _context.SaveChanges();

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = clientRole.Id
        };
        _context.UserRoles.Add(userRole);
        _context.SaveChanges();
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(

            "FirstTest@example.com",
            "FirstUser",
            "First",
            "User",
            "Password123!",
            _existingRoleId
        );

        Result result = await _registerUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameEmail()
    {
        var command = new RegisterUserCommand(

            "SecondTest@example.com",
            "SecondUser",
            "Second",
            "User",
            "Password123!",
            _existingRoleId
        );

        Result result = await _registerUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();

        Result errorResult = await _registerUserCommandHandler.Handle(command, CancellationToken.None);

        errorResult.IsSuccess.ShouldBeFalse();
        Assert.Equal("EmailTaken", errorResult.Error.Code);
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnError_TheSameUsername()
    {
        var command = new RegisterUserCommand(

            "ThirdTest@example.com",
            "ThirdTest",
            "Third",
            "Test",
            "Password123!",
            _existingRoleId
        );

        Result result = await _registerUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();


        var errorCommand = new RegisterUserCommand(

            "NewThirdTest@example.com",
            "ThirdTest",
            "Third",
            "Test",
            "Password123!",
            _existingRoleId
        );
     
        Result errorResult = await _registerUserCommandHandler.Handle(errorCommand, CancellationToken.None);

        errorResult.IsSuccess.ShouldBeFalse();
        Assert.Equal("UsernameTaken", errorResult.Error.Code);
    }

    [Fact]
    public async Task LoginTest_ShouldReturnOk()
    {
        var command = new LoginUserCommand(
            "EndpointTest",
            "Password123!"
        );

        Result result = await _loginUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task LoginTest_ShouldReturnNotFound_WrongUsername()
    {
        var command = new LoginUserCommand(
            "EndpointTestWrong",
            "Password123!"
        );

        Result result = await _loginUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task LoginTest_ShouldReturnUnauthorized_WrongPassword()
    {
        var command = new LoginUserCommand(
            "EndpointTest",
            "Wrong123!"
        );

        Result result = await _loginUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    public void Dispose() => _context.Dispose();
}
