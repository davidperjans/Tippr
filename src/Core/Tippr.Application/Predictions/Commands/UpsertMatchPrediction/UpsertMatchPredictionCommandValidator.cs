using FluentValidation;

namespace Tippr.Application.Predictions.Commands.UpsertMatchPrediction
{
    public class UpsertMatchPredictionCommandValidator : AbstractValidator<UpsertMatchPredictionCommand>
    {
        public UpsertMatchPredictionCommandValidator()
        {
            RuleFor(x => x.MatchId)
                .NotEmpty();

            RuleFor(x => x.PredictedHomeScore)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PredictedAwayScore)
                .GreaterThanOrEqualTo(0);
        }
    }
}
