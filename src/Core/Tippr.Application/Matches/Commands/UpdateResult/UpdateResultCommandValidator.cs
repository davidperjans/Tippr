using FluentValidation;

namespace Tippr.Application.Matches.Commands.UpdateResult
{
    public class UpdateResultCommandValidator : AbstractValidator<UpdateResultCommand>
    {
        public UpdateResultCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.HomeScore)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Home score must be 0 or greater.");

            RuleFor(x => x.AwayScore)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Away score must be 0 or greater.");
        }
    }
}
