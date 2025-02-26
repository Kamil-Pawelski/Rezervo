namespace Domain.Common;

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    BadRequest = 2,
    Problem = 3,
    NotFound = 4,
    Conflict = 5,
    Unauthorized = 6
}
