using FluentValidation;

namespace Tippr.Application.Matches.Commands.CreateMatch
{
    public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
    {
        public CreateMatchCommandValidator()
        {
            RuleFor(x => x.TournamentId).NotEmpty();
            RuleFor(x => x.HomeTeamId).NotEmpty();
            RuleFor(x => x.AwayTeamId).NotEmpty();
            RuleFor(x => x.KickoffUtc).GreaterThan(DateTime.UtcNow.AddMinutes(-5));

            RuleFor(x => x)
                .Must(x => x.HomeTeamId != x.AwayTeamId)
                .WithMessage("Home team and away team cannot be the same.");
        }
    }
}
