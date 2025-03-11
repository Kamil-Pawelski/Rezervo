using System.Text.Json.Serialization;

namespace Tests.Response;

public sealed class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = string.Empty;
}
