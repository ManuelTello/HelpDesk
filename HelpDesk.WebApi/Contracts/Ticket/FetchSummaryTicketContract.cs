using System.Text.Json.Serialization;

namespace HelpDesk.WebApi.Contracts.Ticket;

public class FetchSummaryTicketContract
{
    [JsonPropertyName("page")]
    public int Page { get; init; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; init; }

    [JsonPropertyName("order")]
    public string Order { get; init; } = string.Empty;
}
