namespace HelpDesk.WebApi.Responses.Ticket;

public record FetchSummaryTicketResponse(int Page, int PageSize, int TotalTickets, SummaryItem[] Summary);

public record SummaryItem(string Id, string Fullname, string? OwnedBy, string Subject, string Priority, string Category, string State);