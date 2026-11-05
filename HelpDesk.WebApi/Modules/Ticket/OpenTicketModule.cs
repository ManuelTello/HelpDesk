using Carter;
using FluentValidation;
using FluentValidation.Results;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.Ticket;

public class OpenTicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ticket/{id}/open", async (string id, ITicketService service, IValidator<OperationTicketContract> validator) =>
        {
            OperationTicketContract ticketContract = new OperationTicketContract() { Id = id };
            ValidationResult validation = validator.Validate(ticketContract);
            if (validation.IsValid)
            {
                await service.OpenTicket(Guid.Parse(id));
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
        }).RequireAuthorization(policy => policy.RequireRole("Actor"))
        .WithTags("Ticket");
    }
}
