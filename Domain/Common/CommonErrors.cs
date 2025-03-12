namespace Domain.Common;

public static class CommonErrors
{
    public static readonly Error Unauthorized = Error.Unauthorized("Unauthorized", "You are unauthorized to do this action.");
}
