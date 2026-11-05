using HelpDesk.DAL.Models;

namespace HelpDesk.Service.Dto;

public record TicketSummaryDto(TicketSummaryItemDto[] Summary, int Page, int PageSize, int TotalAmount);

public record TicketSummaryItemDto(Guid Id, string Fullname, string? OwnedBy, string Subject, TicketPriority Priority, TicketCategory Category, TicketState State);
