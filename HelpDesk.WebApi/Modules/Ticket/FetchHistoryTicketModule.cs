using Carter;
using FluentValidation;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.Ticket;
using HelpDesk.WebApi.Responses.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.Ticket;

public class FetchHistoryTicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ticket/history", async ([AsParameters] FetchHistoryTicketContract ticketContract, ITicketService service, IValidator<FetchHistoryTicketContract> validator) =>
        {
            var validation = validator.Validate(ticketContract);
            if (validation.IsValid)
            {
                var result = await service.FetchTicketHistory(Guid.Parse(ticketContract.Id), ticketContract.Order);
                FetchHistoryTicketResponse ticketResponse = new FetchHistoryTicketResponse(result.History.Select(h => new HistoryItemResponse(
                    h.DateTimeCreated.ToString("d/M/yyyy hh:mm"),
                    h.State.ToString()))
                .ToArray());
                return Results.Ok(ticketResponse);
            }
            else
            {
                return Results.BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { { "Errors", validation.Errors.ToDictionary(e => e.PropertyName, e => (Object?)e.ErrorMessage) } }
                });
            }
        }).RequireAuthorization(policy => policy.RequireRole("Agent"))
        .WithTags("Ticket")
        .WithName("Fetches the ticket history summary")
        .Produces<FetchHistoryTicketResponse>(StatusCodes.Status200OK);
    }
}
