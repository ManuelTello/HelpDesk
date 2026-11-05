using System.Security.Claims;
using Carter;
using FluentValidation;
using FluentValidation.Results;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.Ticket;

public class TakeTicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ticket/{id}/take", async (string id, ITicketService service, IValidator<OperationTicketContract> validator, HttpContext context) =>
        {
            OperationTicketContract ticketContract = new OperationTicketContract() { Id = id };
            ValidationResult validation = validator.Validate(ticketContract);
            if (validation.IsValid)
            {
                Guid currentUser = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.TakeTicket(Guid.Parse(ticketContract.Id), currentUser);
                return Results.Ok();
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
        .WithTags("Ticket");
    }
}
