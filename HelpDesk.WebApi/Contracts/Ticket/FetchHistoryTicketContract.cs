using System.Text.Json.Serialization;

namespace HelpDesk.WebApi.Contracts.Ticket;

public class FetchHistoryTicketContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("order")]
    public string Order { get; init; } = string.Empty;
}