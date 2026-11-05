using System.Text.Json.Serialization;

namespace HelpDesk.WebApi.Contracts.Ticket;

public class OperationTicketContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = String.Empty;
}
