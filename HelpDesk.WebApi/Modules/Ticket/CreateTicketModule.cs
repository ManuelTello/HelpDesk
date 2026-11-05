using System.Security.Claims;
using Carter;
using FluentValidation;
using HelpDesk.DAL.Models;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.Ticket;

public class CreateTicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ticket/create", async ([FromBody] CreateTicketContract ticketContract, ITicketService ticketService, IValidator<CreateTicketContract> validator, HttpContext context) =>
        {
            var validation = validator.Validate(ticketContract);
            if (validation.IsValid)
            {
                TicketCategory category = Enum.Parse<TicketCategory>(ticketContract.Category);
                TicketPriority priority = Enum.Parse<TicketPriority>(ticketContract.Priority);
                Guid currentUser = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await ticketService.CreateTicket(ticketContract.Subject, ticketContract.Description, category, priority, currentUser);
                return Results.Created();
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
        .WithTags("Ticket")
        .WithName("Creates a new ticket.")
        .Accepts<CreateTicketContract>("application/json")
        .Produces(StatusCodes.Status201Created);
    }
}
