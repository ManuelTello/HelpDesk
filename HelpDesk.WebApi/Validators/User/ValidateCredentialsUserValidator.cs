using FluentValidation;
using HelpDesk.WebApi.Contracts.User;

namespace HelpDesk.WebApi.Validators.User;

public class ValidateCredentialsUserValidator : AbstractValidator<ValidateCredentialsUserContract>
{
    public ValidateCredentialsUserValidator()
    {
        RuleFor(x => x.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull();
    }
}
