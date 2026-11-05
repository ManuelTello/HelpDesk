using FluentValidation;
using HelpDesk.WebApi.Contracts.Ticket;

namespace HelpDesk.WebApi.Validators.Ticket;

public class IdValidator : AbstractValidator<OperationTicketContract>
{
    public IdValidator()
    {
        RuleFor(x => x.Id).Must(IsIdValid).WithMessage("Invalid id");
    }

    public bool IsIdValid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}