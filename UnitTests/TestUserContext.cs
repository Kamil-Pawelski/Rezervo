using Application.Abstractions.Authentication;

namespace Tests;

public class TestUserContext() : IUserContext
{
    public Guid UserId { get; set; }
}
