using Domain.Common;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error EmailTaken = Error.Conflict("EmailTaken", "The provided email is already in use.");
    public static readonly Error UsernameTaken = Error.Conflict("UsernameTaken", "The chosen username is already taken.");
    public static readonly Error NotFoundRole = Error.NotFound("NotFoundRole", "The specified role does not exist.");
    public static readonly Error NotFoundUser = Error.NotFound("NotFoundUser", "Invalid username or email.");
    public static readonly Error InvalidPassword = Error.Unauthorized("InvalidPassword", "The provided password is incorrect.");
}

