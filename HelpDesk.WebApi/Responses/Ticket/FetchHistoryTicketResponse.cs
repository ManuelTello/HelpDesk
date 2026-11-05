namespace HelpDesk.WebApi.Responses.Ticket;

public record FetchHistoryTicketResponse(HistoryItemResponse[] History);

public record HistoryItemResponse(string DateCreated, string State);