namespace ServiceArchitecture.Scoring.Events
{
    /// <summary>
    /// Published when the player's score has changed.
    /// </summary>
    public struct ScoreUpdatedEvent
    {
        public int NewTotalScore;
    }
}