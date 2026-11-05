using Carter;
using FluentValidation;
using FluentValidation.Results;
using HelpDesk.Service.Abstractions;
using HelpDesk.WebApi.Contracts.User;
using HelpDesk.WebApi.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.WebApi.Modules.User;

public class ValidateCredentialsUserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/user/validate", async ([FromBody] ValidateCredentialsUserContract contract, IUserService service, IConfiguration configuration, IValidator<ValidateCredentialsUserContract> validator) =>
        {
            ValidationResult validation = validator.Validate(contract);
            if (validation.IsValid)
            {
                string issuer = configuration["Jwt:Issuer"]!;
                string audience = configuration["Jwt:Audience"]!;
                string key = configuration["Jwt:Key"]!;
                double expiration = double.Parse(configuration["Jwt:ExpirationTime"]!);
                var token = await service.ValidateCredentials(contract.UserName, contract.Password, issuer, audience, key, expiration);
                return Results.Ok(new ValidateCredentialsUserResponse(token));
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
        .WithName("Validate credentials")
        .Accepts<ValidateCredentialsUserContract>("application/json")
        .Produces<ValidateCredentialsUserResponse>(StatusCodes.Status200OK);
    }
}
