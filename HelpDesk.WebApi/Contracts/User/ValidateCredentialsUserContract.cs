using System.Text.Json.Serialization;

namespace HelpDesk.WebApi.Contracts.User;

public class ValidateCredentialsUserContract
{
    [JsonPropertyName("username")]
    public string UserName { get; init; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; init; } = string.Empty;
}
