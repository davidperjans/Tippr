using FluentValidation;

namespace Tippr.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.TournamentId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.FifaCode)
                .NotEmpty()
                .Length(2, 4); // t.ex. "SWE", "ENG"
        }
    }
}
