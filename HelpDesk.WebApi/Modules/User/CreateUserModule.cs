using Carter;
using FluentValidation;
using FluentValidation.Results;
using HelpDesk.DAL.Models;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.User;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.User;

public class CreateUserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/user/create", async ([FromBody] CreateUserContract contract, IUserService service, IValidator<CreateUserContract> validator) =>
        {
            ValidationResult validation = validator.Validate(contract);
            if (validation.IsValid)
            {
                await service.CreateUser(contract.UserName, contract.FullName, contract.Password, Enum.Parse<UserRole>(contract.Role));
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
        }).WithTags("User")
        .WithName("Create user")
        .Accepts<CreateUserContract>("application/json")
        .Produces(StatusCodes.Status201Created);
    }
}
