using FluentValidation;
using HelpDesk.WebApi.Contracts.Shared;
using HelpDesk.WebApi.Contracts.Ticket;

namespace HelpDesk.WebApi.Validators.Ticket;

public class FetchHistoryTicketValidator : AbstractValidator<FetchHistoryTicketContract>
{
    public FetchHistoryTicketValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
                .Must(IsIdValid).WithMessage("Invalid id");
        RuleFor(x => x.Order)
            .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(order => Enum.TryParse<OrderType>(order, out _)).WithMessage("Invalid order");
    }

    public bool IsIdValid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
