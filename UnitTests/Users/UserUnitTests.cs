﻿using Application.Users.Login;
using Application.Users.Register;
using Domain.Common;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace Tests.Users;

public sealed class UserUnitTests
{
    private readonly PasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;
    private readonly TokenProvider _tokenProvider;


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
        _tokenProvider = new TokenProvider(configuration, _context);

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

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher).Handle(command, CancellationToken.None);

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

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("EmailTaken");
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

        Result result = await new RegisterUserCommandHandler(_context, _passwordHasher).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("UsernameTaken");
    }

    [Fact]
    public async Task LoginTest_ShouldReturnOk()
    {
        var command = new LoginUserCommand(
            SeedData.TestUsername,
            SeedData.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task LoginTest_ShouldReturnNotFound_WrongUsername()
    {
        var command = new LoginUserCommand(
            "EndpointTestWrong",
            SeedData.TestPassword
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundUser");
    }

    [Fact]
    public async Task LoginTest_ShouldReturnUnauthorized_WrongPassword()
    {
        var command = new LoginUserCommand(
            SeedData.TestUsername,
            "Wrong123!"
        );

        Result result = await new LoginUserCommandHandler(_context, _passwordHasher, _tokenProvider).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("InvalidPassword");
    }

}
