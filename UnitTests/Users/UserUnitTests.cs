using Application.Users;
using Domain.Common;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace Tests.Users;

public class UserUnitTests
{
    private readonly RegisterUserCommandHandler _registerUserCommandHandler;
    private readonly ApplicationDbContext _context;
    public UserUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        _registerUserCommandHandler = new RegisterUserCommandHandler(_context, new PasswordHasher());
    }

    [Fact]
    public async Task RegisterTest_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(

            "FirstTest@example.com",
            "FirstUser",
            "First",
            "User",
            "Password123!"
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
            "Password123!"
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
            "Password123!"
        );

        Result result = await _registerUserCommandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();


        var errorCommand = new RegisterUserCommand(

            "NewThirdTest@example.com",
            "ThirdTest",
            "Third",
            "Test",
            "Password123!"
        );
     
        Result errorResult = await _registerUserCommandHandler.Handle(errorCommand, CancellationToken.None);

        errorResult.IsSuccess.ShouldBeFalse();
        Assert.Equal("UsernameTaken", errorResult.Error.Code);
    }
}
