using HelpDesk.DAL.Models;
using HelpDesk.Service.Dto;

namespace HelpDesk.Service.Abstractions;

public interface ITicketService
{
    public Task CreateTicket(string subject, string description, TicketCategory category, TicketPriority priority, Guid actorId);

    public Task<TicketSummaryDto> FetchTicketsSummary(int page, int pageSize, string orderBy);

    public Task<HistoryDto> FetchTicketHistory(Guid ticketId, string order);

    public Task OpenTicket(Guid ticketId);

    public Task TakeTicket(Guid ticketId, Guid agentId);

    public Task ResolveTicket(Guid ticketId);

    public Task CloseTicket(Guid ticketId);

    public Task CancelTicket(Guid ticketId);
}
