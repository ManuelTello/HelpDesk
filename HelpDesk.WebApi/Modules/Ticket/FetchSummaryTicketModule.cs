using Carter;
using FluentValidation;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.Ticket;
using HelpDesk.WebApi.Responses.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.Ticket;

public class FetchSummaryTicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ticket/summary", async ([AsParameters] FetchSummaryTicketContract ticketContract, IValidator<FetchSummaryTicketContract> validator, ITicketService ticketService) =>
        {
            var validation = validator.Validate(ticketContract);
            if (validation.IsValid)
            {
                var dto = await ticketService.FetchTicketsSummary(ticketContract.Page, ticketContract.PageSize, ticketContract.Order);
                FetchSummaryTicketResponse ticketResponse = new FetchSummaryTicketResponse(
                    dto.Page,
                    dto.PageSize,
                    dto.TotalAmount,
                    dto.Summary.Select(d => new SummaryItem(
                        d.Id.ToString(),
                        d.Fullname,
                        d.OwnedBy,
                        d.Subject,
                        d.Priority.ToString(),
                        d.Category.ToString(),
                        d.State.ToString()))
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
        .WithName("Fetches the ticket summary by order of creation")
        .Produces<FetchSummaryTicketResponse>(StatusCodes.Status200OK);
    }
}
