namespace Tippr.Domain.Enums
{
    public enum PredictionMode
    {
        AllAtOnce = 0,        // Predict all before tournament
        BeforeEatchMatch = 1, // Predict before each match
        StageByStage = 2      // PRedict by tournament stage
    }
}
