using FluentValidation;
using HelpDesk.DAL.Models;
using HelpDesk.WebApi.Contracts.User;

namespace HelpDesk.WebApi.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserContract>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .Length(4, 25)
            .Matches("^([a-zA-Z]{_|.}?)*$").WithMessage("Invalid username");
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .Matches("^[a-zA-Z0-9_,.-]{4,}$").WithMessage("Invalid password");
        RuleFor(x => x.FullName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .Matches("^[a-zA-Z ]+$").WithMessage("Invalid full name");
        RuleFor(x => x.Role)
            .NotEmpty()
            .NotNull()
            .Must(e => Enum.TryParse<UserRole>(e, out _)).WithMessage("Invalid role");
    }
}
