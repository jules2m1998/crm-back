using FluentValidation;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
    public AddEventCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .LessThan(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Topic)
            .NotEmpty();

        RuleFor(x => x.OwnerId)
            .NotEmpty();
    }
}
