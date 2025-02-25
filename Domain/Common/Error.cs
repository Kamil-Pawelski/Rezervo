namespace Domain.Common;

public class Error(string code, string description, ErrorType type)
{
    public string Code { get; } = code;
    public string Description { get; } = description;
    public ErrorType Type { get; } = type;

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);
    public static Error BadRequest(string code, string description) =>
        new(code, description, ErrorType.BadRequest);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);


}
