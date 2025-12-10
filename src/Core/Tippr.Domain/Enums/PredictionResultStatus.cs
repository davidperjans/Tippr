namespace Tippr.Domain.Enums
{
    public enum PredictionResultStatus
    {
        Pending = 0,
        CorrectExactScore = 1,
        CorrectOutcomeAndDiff = 2,
        CorrectOutcome = 3,
        Incorrect = 4
    }
}
