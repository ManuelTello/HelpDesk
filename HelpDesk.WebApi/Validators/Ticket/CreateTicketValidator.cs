using FluentValidation;
using HelpDesk.DAL.Models;
using HelpDesk.WebApi.Contracts.Ticket;

namespace HelpDesk.WebApi.Validators.Ticket;

public class CreateTicketValidator : AbstractValidator<CreateTicketContract>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.Subject).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
        RuleFor(x => x.Description).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .MaximumLength(250);
        RuleFor(x => x.Priority).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .Must(e => Enum.TryParse<TicketPriority>(e, out _)).WithMessage("Invalid priority type");
        RuleFor(x => x.Category).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .Must(e => Enum.TryParse<TicketCategory>(e, out _)).WithMessage("Invalid category type.");
    }
}
