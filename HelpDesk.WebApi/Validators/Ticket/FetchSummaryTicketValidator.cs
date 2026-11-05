using FluentValidation;
using HelpDesk.WebApi.Contracts.Shared;
using HelpDesk.WebApi.Contracts.Ticket;

namespace HelpDesk.WebApi.Validators.Ticket;

public class FetchSummaryTicketValidator : AbstractValidator<FetchSummaryTicketContract>
{
    public FetchSummaryTicketValidator()
    {
        RuleFor(x => x.Page)
            .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .Cascade(CascadeMode.Stop)
                .LessThanOrEqualTo(50)
                .GreaterThan(1)
                .Must(e => e % 5 == 0).WithMessage("Invalid page size.");
        RuleFor(x => x.Order)
            .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .Must(order => Enum.TryParse<OrderType>(order, out _)).WithMessage("Invalid order");
    }
}
