using FluentValidation;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs.Commands.MoveHead;

public class MoveHeadValidator : AbstractValidator<MoveHeadCommand>
{
    public MoveHeadValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.AgentId)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.ResponseId)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.Message)
            .MaximumLength(255);
    }
}
