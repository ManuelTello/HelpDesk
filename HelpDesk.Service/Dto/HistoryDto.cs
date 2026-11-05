using HelpDesk.DAL.Models;

namespace HelpDesk.Service.Dto;

public record HistoryDto(HistoryItemDto[] History);

public record HistoryItemDto(DateTime DateTimeCreated, TicketState State);
