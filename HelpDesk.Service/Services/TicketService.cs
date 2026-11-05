using HelpDesk.DAL.Data;
using HelpDesk.DAL.Models;
using HelpDesk.Service.Abstractions;
using HelpDesk.Service.Dto;
using HelpDesk.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Service.Services;

public class TicketService : ITicketService
{
    private readonly ApplicationContext _dbContext;

    public TicketService(ApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task CreateTicket(string subject, string description, TicketCategory category, TicketPriority priority, Guid actorId)
    {
        Ticket ticket = new Ticket()
        {
            CreatorId = actorId,
            Owner = null,
            Subject = subject,
            Description = description,
            Category = category,
            Priority = priority,
        };
        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.Open
        });
        await _dbContext.Tickets.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<HistoryDto> FetchTicketHistory(Guid ticketId, string order)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        IEnumerable<HistoryItem> historySorted = order.ToLower() == "desc" ?
            ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated) :
            ticket.HistorySummary.OrderBy(hs => hs.DateTimeCreated);
        return new HistoryDto(historySorted.Select(hs => new HistoryItemDto(hs.DateTimeCreated, hs.State)).ToArray());
    }

    public async Task<TicketSummaryDto> FetchTicketsSummary(int page, int pageSize, string orderBy)
    {
        IQueryable<Ticket> tickets = _dbContext.Tickets;
        if (orderBy == "asc")
        {
            tickets.OrderBy(t => t.DateTimeCreated);
        }
        else
        {
            tickets.OrderByDescending(t => t.DateTimeCreated);
        }

        TicketSummaryItemDto[] queryResult = await tickets
            .Include(t => t.HistorySummary)
            .Include(t => t.Creator)
            .Include(t => t.Owner)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Select(t => new TicketSummaryItemDto(
                t.Id,
                t.Creator.FullName,
                t.Owner == null ? null : t.Owner.FullName,
                t.Subject,
                t.Priority,
                t.Category,
                t.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State)).ToArrayAsync();
        int amountOfTickets = await _dbContext.Tickets.CountAsync();
        return new TicketSummaryDto(queryResult, page, pageSize, amountOfTickets);
    }

    public async Task OpenTicket(Guid ticketId)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        TicketState actualState = ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State;

        if (actualState is not TicketState.Resolved)
        {
            throw new TicketOperationException();
        }

        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.InProgress
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task CancelTicket(Guid ticketId)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        TicketState actualState = ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State;

        if (actualState is TicketState.Closed)
        {
            throw new TicketOperationException();
        }

        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.Cancelled
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task CloseTicket(Guid ticketId)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        TicketState actualState = ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State;

        if (actualState is not TicketState.Resolved)
        {
            throw new TicketOperationException();
        }

        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.Closed
        });
        ticket.DateTimeClosed = DateTime.Now;

        await _dbContext.SaveChangesAsync();
    }

    public async Task ResolveTicket(Guid ticketId)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        TicketState actualState = ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State;

        if (actualState is not TicketState.InProgress)
        {
            throw new TicketOperationException();
        }

        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.Resolved
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task TakeTicket(Guid ticketId, Guid actorId)
    {
        Ticket ticket = await _dbContext.Tickets
            .Include(t => t.HistorySummary)
            .SingleOrDefaultAsync(t => t.Id == ticketId) ?? throw new TicketNotFoundException();
        TicketState actualState = ticket.HistorySummary.OrderByDescending(hs => hs.DateTimeCreated).First().State;

        if (actualState is not TicketState.Open && actualState is not TicketState.Resolved)
        {
            throw new TicketOperationException();
        }

        ticket.HistorySummary.Add(new HistoryItem()
        {
            DateTimeCreated = DateTime.Now,
            State = TicketState.InProgress
        });
        ticket.OwnerId = actorId;

        await _dbContext.SaveChangesAsync();
    }
}